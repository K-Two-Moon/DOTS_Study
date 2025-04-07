using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 掉落系统
/// </summary>
public partial struct Day6PropSystem : ISystem
{
    /// <summary>
    /// 记录临时拖拽的实体
    /// </summary>
    private Entity dragEntity;
    /// <summary>
    /// 拖拽实体的默认变换组件
    /// </summary>
    private LocalTransform defafultTran;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day6Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //查找到掉落物品
            foreach (var (pTran, upd, pd, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day6UsePropData>, RefRW<Day6PropData>>().WithEntityAccess())
            {
                pd.ValueRW.livedTimer -= SystemAPI.Time.DeltaTime;
                if (pd.ValueRW.livedTimer <= 0)
                {
                    ecb.DestroyEntity(pEntity);
                }
                pTran.ValueRW.Scale += SystemAPI.Time.DeltaTime;
                if (pTran.ValueRW.Scale > pd.ValueRW.maxScale)
                {
                    //查找到所有的敌人
                    foreach (var (et, ehp, ed, eEntity) in SystemAPI.Query<RefRO<Day6EnemyTag>, Day6Hp, Day6AnimObj>().WithEntityAccess())
                    {
                        if (math.distance(pTran.ValueRW.Position, ed.ins.transform.position) < 5)
                        {
                            ecb.DestroyEntity(eEntity);
                        }
                    }
                    ecb.DestroyEntity(pEntity);
                }
            }
            ecb.Playback(state.EntityManager);
        }
        //
        if (Input.GetMouseButtonDown(0))
        {
            //发射射线
            var hit = GetECSHit();
            //判断射线检测的实体是否是道具
            if (state.EntityManager.HasComponent<Day6PropData>(hit.Entity))
            {
                dragEntity = hit.Entity;//记录拖拽的实体
                defafultTran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
            }
        }
        if (Input.GetMouseButton(0))
        {
            //首先判断拖拽的实体是否存在
            if (dragEntity != default)
            {
                //判断拖拽的实体身上是否有变换组件
                if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
                {
                    Debug.Log("hit2");
                    //获取拖拽实体身上的变换组件
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                    //坐标转换
                    var vpos = Camera.main.WorldToScreenPoint(tran.Position);
                    //鼠标位置结合vpos的z重新计算出来一个新的世界坐标
                    var wpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vpos.z));
                    tran.Position = wpos;
                    //应用到实体
                    state.EntityManager.SetComponentData(dragEntity, tran);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //判断拖拽的实体身上是否有变换组件
            if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
            {
                //获取拖拽实体身上的变换组件
                var tran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                if (tran.Position.z < -11)
                {
                    state.EntityManager.SetComponentData(dragEntity, defafultTran);
                }
                else
                {
                    //添加使用标签
                    state.EntityManager.AddComponent<Day6UsePropData>(dragEntity);
                }
            }
            dragEntity = default;
        }
    }

    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. 
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. 
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput() { Start = ray.origin, End = ray.direction * 1000, Filter = CollisionFilter.Default };
        //3. 
        Unity.Physics.RaycastHit hit;
        //4. 
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. 
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit;
    }
    private UnityEngine.RaycastHit GetHit()
    {
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        UnityEngine.RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

        }
        return hit;
    }
}
