using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 士兵组件系统
/// </summary>
public partial struct Day8_9SoldierSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day8_9Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            foreach (var (pTran, pd,  php, pEnity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day8_9SoldierMoveData>,Day8_9HpData>().WithEntityAccess())
            {
                //判断是否到达目标点
                if (math.distance(pTran.ValueRW.Position, pd.ValueRO.targetPos) > 0.5f)
                {
                    //计算移动方向
                    var dir = pd.ValueRO.targetPos - pTran.ValueRW.Position;
                    pTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * 0.5f;
                    //血条跟随
                    php.sli.transform.position = Camera.main.WorldToScreenPoint(pTran.ValueRW.Position + new float3(0, 1, 0));
                }
                else
                {
                    //到达目标点，移除移动组件
                    ecb.RemoveComponent<Day8_9SoldierMoveData>(pEnity);
                }
            }
            //查找到所有的士兵
            foreach (var (pTran, pd, php, pEnity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day8_9SoldierData>, Day8_9HpData>().WithEntityAccess())
            {
                //查找到敌人
                foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day8_9EnemyData>, Day8_9HpData>().WithEntityAccess())
                {
                    //判断敌人是否在士兵的攻击范围内
                    if (math.distance(pTran.ValueRW.Position, eTran.ValueRW.Position) < 2)
                    {
                        pd.ValueRW.attackTime -= SystemAPI.Time.DeltaTime;
                        if (pd.ValueRW.attackTime <= 0)
                        {
                            //敌人掉血
                            ehp.sli.value -= 40;
                            if (ehp.sli.value <= 0)
                            {
                                ecb.DestroyEntity(eEntity);
                            }
                            pd.ValueRW.attackTime = 1;
                        }
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }

    }
}
