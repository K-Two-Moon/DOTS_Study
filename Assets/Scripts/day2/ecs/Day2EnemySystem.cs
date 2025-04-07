using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
/// <summary>
/// ����ʵ�壬������˵�����߼����� 
/// </summary>
public partial struct Day2EnemySystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day2Tag>();  //��ϵͳ�����OnUpdate����ֻ�д��ڸñ�ǩ��ʱ��Ż���и���
    }

    void OnUpdate(ref SystemState state)
    {
        //���ҵ����еĵ���
        foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2EnemyData>, Day2Hp>().WithEntityAccess())
        {   //Ѫ������
            ehp.sli.transform.position = Camera.main.WorldToScreenPoint(eTran.ValueRW.Position + new float3(0, 1, 0));
        }
    }
}
