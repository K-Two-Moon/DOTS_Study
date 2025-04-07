using System.IO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
/// <summary>
/// 士兵系统
/// </summary>
public partial struct Day15_17SoldierSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day15_17Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb=new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        {
            //查找到小兵
            foreach (var (sTran,sd) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day15_17SoldierData>>())
            {
                //查找到所有的敌人
                foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17EnemyData>, Day15_17Hp>().WithEntityAccess())
                {
                    //判断是否在攻击范围内
                    if (math.distance(sTran.ValueRW.Position,eTran.ValueRW.Position)<2)
                    {
                        sd.ValueRW.attackTimer -= SystemAPI.Time.DeltaTime;
                        if (sd.ValueRW.attackTimer<=0)
                        {
                            //敌人掉血
                            ehp.sli.value -= sd.ValueRW.attackValue;
                            if (ehp.sli.value<=0)
                            {
                                ecb.DestroyEntity(eEntity);
                            }
                            sd.ValueRW.attackTimer = 1;
                        }
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}
