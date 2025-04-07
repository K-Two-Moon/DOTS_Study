using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

public partial struct Day14PlayerSystem : ISystem
{
    private int count;
    private Day14InsData insData;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day14Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        //����ʵ�������
        foreach (var item in SystemAPI.Query<RefRW<Day14InsData>>())
        {
            insData = item.ValueRO;
        }
        if (count == 0)
        {
            var pEnity = state.EntityManager.Instantiate(insData.player);
            var playerPos = new Vector3(0, 0, -4);
            state.EntityManager.SetComponentData(pEnity, new LocalTransform { Position = playerPos, Scale = 1 });
            state.EntityManager.AddComponentData(pEnity, new Day14PlayerData { hp = 100 });
            count = 1;
        }
        //���ҵ����
        foreach (var (pTran, pd, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day14PlayerData>>().WithEntityAccess())
        {
            float h = ETC.ins.GetAxis("H");
            float v = ETC.ins.GetAxis("V");
            Vector3 pos = new Vector3(h, 0, v);
            if (pos != Vector3.zero)
            {
                //������ֵ���¸�ֵ�����ҵ���LocalTransform,Ҳ���������pTran
                LocalTransform tran = pTran.ValueRW.Translate(new Unity.Mathematics.float3(h, 0, v) * 10 * Time.deltaTime);
                pTran.ValueRW.Position = tran.Position;
            }
        }
    }
}
