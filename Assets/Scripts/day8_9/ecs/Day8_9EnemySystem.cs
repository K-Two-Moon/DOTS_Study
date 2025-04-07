using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// ����ϵͳ
/// </summary>
public partial struct Day8_9EnemySystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day8_9Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //���ҵ�����
            foreach (var (eTran,ed,ehp,eEntity) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day8_9EnemyData>,Day8_9HpData>().WithEntityAccess())
            {
                //�����ж��Ƿ�������
                if (Day8_9GameRoot.ins.hasMainCity)
                {
                    //�ж��Ƿ񵽴�Ŀ��λ��
                    if (math.distance(eTran.ValueRW.Position,ed.ValueRW.targetPos)>0.5f)
                    {
                        //�����ƶ�����
                        var dir = ed.ValueRW.targetPos - eTran.ValueRW.Position;
                        eTran.ValueRW.Position += dir*SystemAPI.Time.DeltaTime*0.1f;
                        //�ж��Ƿ񴥷��˲t�����ľ���ʱ��
                        if (math.abs(eTran.ValueRW.Position.x-8)<1)
                        {
                            Day8_9GameRoot.ins.canShowViewTime = true;
                        }
                    }
                    //Ѫ������
                    ehp.sli.transform.position = Camera.main.WorldToScreenPoint(eTran.ValueRW.Position+new float3(0,1,0));
                }
                //���ҵ�����
                foreach (var (bTran,bd,bhp,bEntity) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day8_9BuildingData>,Day8_9HpData>().WithEntityAccess())
                {
                    //�����ж�������
                    if (bd.ValueRW.bType== Day8_9BuilingType.MainCity)
                    {
                        //�жϵ��˺����ǵ�λ��
                        if (math.distance(eTran.ValueRW.Position, bTran.ValueRW.Position) < 4)
                        {
                            ed.ValueRW.attackTime -= SystemAPI.Time.DeltaTime;
                            if (ed.ValueRW.attackTime <= 0)
                            {
                                bhp.sli.value -= 1;
                                if (bhp.sli.value<=0)
                                {
                                    ecb.DestroyEntity(bEntity);
                                    Time.timeScale = 0;
                                }
                                ed.ValueRW.attackTime = 2;
                            }
                        }
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
    } 
}
