using System.IO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
/// <summary>
/// ʿ��ϵͳ
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
            //���ҵ�С��
            foreach (var (sTran,sd) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day15_17SoldierData>>())
            {
                //���ҵ����еĵ���
                foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17EnemyData>, Day15_17Hp>().WithEntityAccess())
                {
                    //�ж��Ƿ��ڹ�����Χ��
                    if (math.distance(sTran.ValueRW.Position,eTran.ValueRW.Position)<2)
                    {
                        sd.ValueRW.attackTimer -= SystemAPI.Time.DeltaTime;
                        if (sd.ValueRW.attackTimer<=0)
                        {
                            //���˵�Ѫ
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
