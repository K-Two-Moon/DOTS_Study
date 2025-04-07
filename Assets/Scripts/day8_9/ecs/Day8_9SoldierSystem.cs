using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// ʿ�����ϵͳ
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
                //�ж��Ƿ񵽴�Ŀ���
                if (math.distance(pTran.ValueRW.Position, pd.ValueRO.targetPos) > 0.5f)
                {
                    //�����ƶ�����
                    var dir = pd.ValueRO.targetPos - pTran.ValueRW.Position;
                    pTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * 0.5f;
                    //Ѫ������
                    php.sli.transform.position = Camera.main.WorldToScreenPoint(pTran.ValueRW.Position + new float3(0, 1, 0));
                }
                else
                {
                    //����Ŀ��㣬�Ƴ��ƶ����
                    ecb.RemoveComponent<Day8_9SoldierMoveData>(pEnity);
                }
            }
            //���ҵ����е�ʿ��
            foreach (var (pTran, pd, php, pEnity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day8_9SoldierData>, Day8_9HpData>().WithEntityAccess())
            {
                //���ҵ�����
                foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day8_9EnemyData>, Day8_9HpData>().WithEntityAccess())
                {
                    //�жϵ����Ƿ���ʿ���Ĺ�����Χ��
                    if (math.distance(pTran.ValueRW.Position, eTran.ValueRW.Position) < 2)
                    {
                        pd.ValueRW.attackTime -= SystemAPI.Time.DeltaTime;
                        if (pd.ValueRW.attackTime <= 0)
                        {
                            //���˵�Ѫ
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
