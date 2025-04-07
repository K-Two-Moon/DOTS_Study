using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 玩家系统
/// </summary>
public partial struct Day15_17PlayerSystem : ISystem
{
    //注意：这里的成员变量只能是私有的值类型
    private float h, v;
    private Entity dragEntity;
    private float recoverHp;
    private float3 playerPos;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day15_17Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //查找到玩家
            foreach (var (pTran, pt, php, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17PlayerTag>, Day15_17Hp>().WithEntityAccess())
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                if (h != 0 || v != 0)
                {
                    pTran.ValueRW.Position += new Unity.Mathematics.float3(h, 0, v) * SystemAPI.Time.DeltaTime * 10;
                }
                playerPos = pTran.ValueRW.Position;//记录玩家的位置
                //血条跟随
                php.sli.transform.position = Camera.main.WorldToScreenPoint((Vector3)pTran.ValueRW.Position + Vector3.up);
                //查找到建筑
                foreach (var (bTran, bd, bhp, bEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17BuildingData>, Day15_17Hp>().WithEntityAccess())
                {
                    //判断是否是主城
                    if (bd.ValueRW.bName == "回血塔")
                    {
                        //判断是否到达回血塔附近
                        if (math.distance(pTran.ValueRW.Position,bTran.ValueRW.Position)<2)
                        {
                            recoverHp += SystemAPI.Time.DeltaTime;
                            if (recoverHp>=1)
                            {
                                php.sli.value += 1;
                                recoverHp = 0;
                            }
                        }
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }

        //鼠标按下
        if (Input.GetMouseButtonDown(1))
        {

            var hit = GetEscHit();
            //判断是建筑
            if (state.EntityManager.HasComponent<Day15_17BuildingData>(hit.Entity))
            {
                //获取建筑组件
                var data = state.EntityManager.GetComponentData<Day15_17BuildingData>(hit.Entity);
                if (data.bName == "兵工厂")
                {
                    //获取兵工厂的位置
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //显示创建造按钮
                    Day15_17GameRoot.ins.ShowCreateUI(tran.Position);
                }
            }
        }

        //鼠标按下
        if (Input.GetMouseButtonDown(0))
        {
            //判断ui事件检测到的游戏对象不为null
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                //当前选中的对象不是建造按钮，则可以进行拖拽移动
                if (EventSystem.current.currentSelectedGameObject.tag == "uitag")
                {
                    Debug.Log("|-----------|");
                    return;
                }
            }
            var hit = GetEscHit();
            //判断是建筑
            if (state.EntityManager.HasComponent<Day15_17BuildingData>(hit.Entity))
            {
                //获取建筑组件
                var data = state.EntityManager.GetComponentData<Day15_17BuildingData>(hit.Entity);
                if (data.bName == "兵工厂")
                {
                    Debug.Log("-----------");
                    dragEntity = hit.Entity;
                }
            }

        }
        //鼠标按住不松开
        if (Input.GetMouseButton(0))
        {
            //判断拖拽的实体是否存在
            if (dragEntity != default)
            {
                //判断拖拽的实体身上是否有变化组件
                if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
                {
                    //获取拖拽实体身上的变换组件
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                    //计算
                    var vPos = Camera.main.WorldToScreenPoint(tran.Position);
                    //
                    var wPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vPos.z));
                    //赋值
                    tran.Position = wPos;
                    state.EntityManager.SetComponentData(dragEntity, tran);
                    //获取实体身上的血条托管组件
                    var hp = state.EntityManager.GetComponentData<Day15_17Hp>(dragEntity);
                    hp.sli.transform.position = Camera.main.WorldToScreenPoint((Vector3)tran.Position + Vector3.up);
                }
            }
        }

        //松开鼠标
        if (Input.GetMouseButtonUp(0))
        {
            dragEntity = default;
        }
        if (Input.GetKeyDown(KeyCode.Space))//空格键按下 
        {
            var en = GetColosetEnemy(state.EntityManager,2);
            state.EntityManager.DestroyEntity(en);
        }
    }

    /// <summary>
    /// 获取最近的攻击的实体
    /// </summary>
    private Entity GetColosetEnemy(EntityManager em,float attackValue)
    {
        //记录最近的距离
        float minDistance = float.MaxValue;
        //最近敌人的索引
        int index = 0;
        //记录查找到的最近的实体
        Entity en = default;
        //构建eqb实体查询器
        var query = new EntityQueryBuilder(Unity.Collections.Allocator.Temp).WithAll<Day15_17EnemyData>().Build(em);
        //转换为实体数组
        var array = query.ToEntityArray(Unity.Collections.Allocator.Temp);
        for (int i = 0; i < array.Length; i++)
        {
            //获取实体身上的变换组件
            var tran = em.GetComponentData<LocalTransform>(array[i]);
            //计算敌人和玩家之间的距离
            var dis = math.distance(playerPos,tran.Position);
            if (dis<attackValue)//在玩家的攻击范围内
            {
                if (dis < minDistance)
                {
                    minDistance = dis;
                    index = i;
                }
            }
        }
        //确保数组不越界
        if (array.Length>index)
        {
            en= array[index]; 
        }
        return en;
    }
    private Unity.Physics.RaycastHit GetEscHit()
    {
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput { Start = ray.origin, End = ray.direction * 1000, Filter = CollisionFilter.Default };
        Unity.Physics.RaycastHit hit;
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit;
    }
}
