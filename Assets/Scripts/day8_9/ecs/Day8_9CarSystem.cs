using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 小车系统
/// </summary>
public partial struct Day8_9CarSystem : ISystem
{
    private float3 targetPos;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day8_9Tag>();
        targetPos = new float3(-19,0,0);
    }
    void OnUpdate(ref SystemState state)
    {
        //查找到小车
        foreach (var (cTran,ct) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day8_9CarTag>>())
        {
            //计算移动方向
            var dir = targetPos - cTran.ValueRW.Position;
            cTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * 0.5f;
        }
    }
}
