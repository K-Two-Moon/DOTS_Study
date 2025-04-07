using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 定义玩家切面
/// </summary>
readonly partial struct Day3_4PlayerAspect:IAspect
{
    public readonly Entity pEntity;
    public readonly RefRW<LocalTransform> pTran;
    public readonly RefRO<Day3_4PlayerTag> pt;
}

/// <summary>
/// 玩家系统
/// </summary>
public partial struct Day3_4PlayerSystem : ISystem
{
    private float h, v;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day3_4Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        //using (EntityCommandBuffer ecb = new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        //{
        //    //查找到玩家
        //    foreach (var (pTran, pt, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Day3_4PlayerTag>>().WithEntityAccess())
        //    {
        //        if (Day3_4GameRoot.ins.toggle.isOn)
        //        {
        //            h += Input.GetAxis("Horizontal") * SystemAPI.Time.DeltaTime * 10;
        //            v = Input.GetAxis("Vertical");
        //            if (h != 0 || v != 0)
        //            {
        //                if (v > 0)
        //                {
        //                    pTran.ValueRW.Position += pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
        //                }
        //                if (v < 0)
        //                {
        //                    pTran.ValueRW.Position -= pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
        //                }
        //                //旋转
        //                pTran.ValueRW.Rotation = quaternion.RotateY(h);
        //                //摄像机跟随
        //                Camera.main.transform.position = pTran.ValueRW.Position + new float3(0, 2, -6);
        //                var dir = pTran.ValueRW.Position - (float3)Camera.main.transform.position;
        //                Camera.main.transform.rotation = Quaternion.LookRotation(dir);
        //            }
        //        }
        //        //更新玩家的位置
        //        Day3_4GameRoot.ins.posDes.text = $"玩家位置：{(Vector3)pTran.ValueRW.Position}";
        //        //查找到敌人
        //        foreach (var (eTran, et, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day3_4EnemyTag>>().WithEntityAccess())
        //        {
        //            //判断距离
        //            if (math.distance(pTran.ValueRW.Position, eTran.ValueRW.Position) < 1f)
        //            {
        //                ecb.DestroyEntity(eEntity);
        //            }
        //        }
        //    }
        //    ecb.Playback(state.EntityManager);
        //}
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //查找到玩家
            foreach (var  pa in SystemAPI.Query<Day3_4PlayerAspect>())
            {
                if (Day3_4GameRoot.ins.toggle.isOn)
                {
                    h += Input.GetAxis("Horizontal") * SystemAPI.Time.DeltaTime * 10;
                    v = Input.GetAxis("Vertical");
                    if (h != 0 || v != 0)
                    {
                        if (v > 0)
                        {
                            pa. pTran.ValueRW.Position += pa.pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
                        }
                        if (v < 0)
                        {
                            pa.pTran.ValueRW.Position -= pa.pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
                        }
                        //旋转
                        pa.pTran.ValueRW.Rotation = quaternion.RotateY(h);
                        //摄像机跟随
                        Camera.main.transform.position = pa.pTran.ValueRW.Position + new float3(0, 2, -6);
                        var dir = pa.pTran.ValueRW.Position - (float3)Camera.main.transform.position;
                        Camera.main.transform.rotation = Quaternion.LookRotation(dir);
                    }
                }
                //更新玩家的位置
                Day3_4GameRoot.ins.posDes.text = $"玩家位置：{(Vector3)pa.pTran.ValueRW.Position}";
                //查找到敌人
                foreach (var (eTran, et, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day3_4EnemyTag>>().WithEntityAccess())
                {
                    //判断距离
                    if (math.distance(pa.pTran.ValueRW.Position, eTran.ValueRW.Position) < 1f)
                    {
                        ecb.DestroyEntity(eEntity);
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}
