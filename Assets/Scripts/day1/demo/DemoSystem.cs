using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// ϵͳ�������߼���ʵ��
/// </summary>
public partial struct DemoSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DemoDataTag>();//��Ҫ�иñ�ǩ���ڲŻ�ִ��OnUpdate����Ĵ���
    }

    void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;//�÷���ֻ����1��
        //�������
        foreach (var item in SystemAPI.Query<RefRW<DemoInsData>>())
        {
            //ʵ����cube
            state.EntityManager.Instantiate(item.ValueRW.insCube);
        }
    }
}
