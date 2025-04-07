using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// �������ϵͳ��������ҵ�����߼�
/// </summary>
public partial struct Day2PlayerSystem : ISystem
{
    private Day2InsData data;
    private float h, v;
    private float3 playerPos;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day2Tag>();  //��ϵͳ�����OnUpdate����ֻ�д��ڸñ�ǩ��ʱ��Ż���и���
    }

    void OnUpdate(ref SystemState state)
    {
        //����ʵ�������
        foreach (var item in SystemAPI.Query<RefRO<Day2InsData>>())
        {
            data = item.ValueRO;
        }
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp))
        {
            /*
       * pTran:���ܲ��ҵ���LocalTransform���
       * pd:���ܲ��ҵ���Day2PlayerData���
       * pEntity������ʵ������ͬʱӵ��LocalTransform��Day2PlayerData�����ʵ��
       */
            //���ҵ����
            foreach (var (pTran, pd,php, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2PlayerData>,Day2Hp>().WithEntityAccess())
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                if (h != 0 || v != 0)
                {
                    //����ƶ�
                    pTran.ValueRW.Position += new Unity.Mathematics.float3(h, 0, v) * SystemAPI.Time.DeltaTime * pd.ValueRW.moveSpeed;
                    //���������
                    Camera.main.transform.position = pTran.ValueRW.Position + new Unity.Mathematics.float3(0, 2, -4);
                }
                playerPos = pTran.ValueRW.Position;
                //Ѫ������
                php.sli.transform.position = Camera.main.WorldToScreenPoint(pTran.ValueRW.Position+new float3(0,1,0));
                //���ҵ�������Ʒ
                foreach (var (dTran, dt, dEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2DropTag>>().WithEntityAccess())
                {
                    //�жϾ���
                    if (math.distance(pTran.ValueRW.Position, dTran.ValueRW.Position) < 1f)
                    {
                        //���ٵ�����Ʒ
                        ecb.DestroyEntity(dEntity);
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
      
        //���¿ո����ӵ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�����ӵ�ʵ��
            var bEntity = state.EntityManager.Instantiate(data.biu);
            //�����ӵ���λ��
            state.EntityManager.SetComponentData(bEntity, new LocalTransform {  Position=playerPos, Scale=0.5f});
            //Ϊ�ӵ�������
            state.EntityManager.AddComponentData(bEntity, new Day2BiuData {  livedTimer=2, moveSpeed=10});
        }
    }
}
