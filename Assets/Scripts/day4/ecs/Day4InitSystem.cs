using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using static UnityEditor.Progress;

/// <summary>
/// 初始化
/// </summary>
public partial struct Day4InitSystem : ISystem
{
    private Unity.Mathematics.Random rnd;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day4Tag>();
        rnd = new Unity.Mathematics.Random(state.GlobalSystemVersion);
    }
    void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        using (EntityCommandBuffer ecb=new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        {
            //查找缓冲组件
            foreach (var item in SystemAPI.Query<DynamicBuffer<Day4BufferData>>())
            {
                for (global::System.Int32 i = 0; i < item.Length; i++)
                {
                    //随机位置
                    //var rndPos = rnd.NextFloat3();
                    var rndPos = rnd.NextFloat3(new Unity.Mathematics.float3(-9, 0.5f, -1), new Unity.Mathematics.float3(9, 0.5f, 10));
                    //生成实体 
                    var entity = state.EntityManager.Instantiate(item[i].entity);
                    state.EntityManager.SetComponentData(entity, new LocalTransform { Position = rndPos, Scale = 1 });
                  ecb.AddComponent(entity, new Day4HpData { hp = 100 });
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}

/// <summary>
/// 系统
/// </summary>
public partial struct Day4System : ISystem
{
    /// <summary>
    /// 记录拖拽的实体
    /// </summary>
    private Entity dragEntity;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day4Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        //查找到所有的带有生命值的实体
        foreach (var (dhd,dmm,dEntity) in SystemAPI.Query<RefRW<Day4HpData>,RefRW<MaterialMeshInfo>>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState).WithEntityAccess())
        {
            //如何通过代码控制可启用组件？？
            if (Input.GetKeyDown(KeyCode.A))
            {
                state.EntityManager.SetComponentEnabled<Day4HpData>(dEntity, false);
                state.EntityManager.SetComponentEnabled<MaterialMeshInfo>(dEntity, false);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                state.EntityManager.SetComponentEnabled<Day4HpData>(dEntity, true);
                state.EntityManager.SetComponentEnabled<MaterialMeshInfo>(dEntity, true);
            }
            //判断实体身上启用租金是否开启
            if (state.EntityManager.IsComponentEnabled<Day4HpData>(dEntity))
            {
                dhd.ValueRW.hp -= SystemAPI.Time.DeltaTime;
            }
          
        }

        //鼠标按下
        if (Input.GetMouseButtonDown(0))
        {
            var hit = GetECSHit();
            //state.EntityManager.DestroyEntity(hit.Entity);
            dragEntity = hit.Entity;//记录选中的实体
        }
        //鼠标按住不松开
        if (Input.GetMouseButton(0))
        {
            //判断拖拽的实体身上是否有localtransfrom
            if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
            {
                //获取拖拽实体的变换
                var  tran=state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                //坐标转换
                var vpos=Camera.main.WorldToScreenPoint(tran.Position);
                //鼠标位置和vpos的z值，重新计算世界坐标
                var wpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vpos.z));
                tran.Position = wpos;
                //重新映射到拖拽的实体
                state.EntityManager.SetComponentData(dragEntity, tran); 
            }

        }
        //鼠标松开
        if (Input.GetMouseButtonUp(0))
        {
            dragEntity = default;
        }
    }
    /// <summary>
    /// ecs里面的射线检测
    /// 注意：转换ecs的游戏对象预设身上必须有碰撞器存在
    /// </summary>
    /// <returns></returns>
    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. 非ecs里面的射线
        UnityEngine.Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs里面的射线输入
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput() {  Start=ray.origin,End=ray.direction*1000, Filter=CollisionFilter.Default};
        //3. ecs里面的射线碰撞
        Unity.Physics.RaycastHit hit;
        //4. 获取物理世界单利
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. 发射射线
        pw.ValueRO.CastRay(raycastInput,out hit);
        return hit; 
    }
}
