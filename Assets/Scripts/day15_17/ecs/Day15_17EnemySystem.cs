using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// ����ϵͳ
/// </summary>
public partial struct Day15_17EnemySystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day15_17Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        //���ҵ�����
        foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17EnemyData>, Day15_17Hp>().WithEntityAccess())
        {
            //�����ƶ�����
            var dir = ed.ValueRW.targetPos - eTran.ValueRW.Position;
            //�Ƿ񵽴���Ŀ��λ��
            if (math.distance(eTran.ValueRW.Position, ed.ValueRW.targetPos) > 0.5f)
            {
                eTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * 0.2f;
                //Ѫ������
                ehp.sli.transform.position=Camera.main.WorldToScreenPoint((Vector3)eTran.ValueRW.Position+Vector3.up);
            }
            else
            {
                //����Ŀ��λ�ÿ�ʼ��������
                //���ҵ�����
                foreach (var (bTran,bd,bhp,bEntity) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day15_17BuildingData>,Day15_17Hp>().WithEntityAccess())
                {
                    //�ж��Ƿ�������
                    if (bd.ValueRW.bName=="����")
                    {
                        ed.ValueRW.attackTimer -= SystemAPI.Time.DeltaTime;
                        if (ed.ValueRW.attackTimer <= 0)
                        {
                            bhp.sli.value -= ed.ValueRW.attackValue;
                            if (bhp.sli.value<=0)
                            {
                                Debug.Log("��Ϸ����");
                                Time.timeScale = 0;
                            }
                            ed.ValueRW.attackTimer = 2;
                        }
                    }
                }
            }
        }
    }
}
