using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 逻辑的系统
/// </summary>
public partial struct Day10LogicSystem : ISystem
{
    private float timer;
    /// <summary>
    /// 临时记录需要进行移动的实体
    /// </summary>
    private Entity moveEntity;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day10Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        timer += SystemAPI.Time.DeltaTime;
        if (timer>=2)
        {
            Day10GameRoot.ins.dCount += 5;
            Day10GameRoot.ins.diamondDes.text = $"钻石：{Day10GameRoot.ins.dCount}";
            timer = 0;
        }
        //按下左键
        if (Input.GetMouseButtonDown(0))
        {
            var hit = GetECSHit();
            //判断点击的是否是建筑
            if (state.EntityManager.HasComponent<Day10BuildingData>(hit.Entity))
            {
                //判断是否兵工厂
                var bdata= state.EntityManager.GetComponentData<Day10BuildingData>(hit.Entity);
                if (bdata.des=="barrack")
                {
                    //获取点击的兵工厂的位置
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //弹出ui
                    Day10GameRoot.ins.ShowSoldierUI(tran.Position);

                }
            }
            //判断选中的是否是士兵
            if (state.EntityManager.HasComponent<Day10SoldierData>(hit.Entity))
            {
                moveEntity = hit.Entity;
            }
        }
        //右键点击
        if (Input.GetMouseButtonDown(1))
        {
            if (moveEntity!=default)
            {
                var hitPos = GetHitPos();
                //by way1
                state.EntityManager.SetComponentData(moveEntity, new Day10SoldierData { targetPos = hitPos, hp = 100 });
                //by way2
                //state.EntityManager.AddComponentData(moveEntity, new Day10MoveData {  targetPos=hitPos});
            }

            moveEntity = default;
        }
    }

    private Vector3 GetHitPos()
    {
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        UnityEngine.RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {

        }
        return hit.point;
    }
    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. 非ecs里面的射线
        UnityEngine.Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs里面的射线输入
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput {  Start=ray.origin, End=ray.direction*1000, Filter=CollisionFilter.Default};
        //3. ecs里面的检测输出
        Unity.Physics.RaycastHit hit;
        //4. 获取物理单利
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. 射线检测
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit; 
    }
}
