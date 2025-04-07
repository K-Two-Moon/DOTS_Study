using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
/// <summary>
/// ���ϵͳ
/// </summary>
public partial struct Day6PlayerSystem : ISystem
{
    private Day6InsData data;
    private int flag;
    private float h;
    private int changeFlag;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day6Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        //�������
        foreach (var item in SystemAPI.Query<RefRO<Day6InsData>>())
        {
            data = item.ValueRO;
        }
        if (flag == 0)
        {
            var pEntity = state.EntityManager.Instantiate(data.player);
            var pPos = new Vector3(0, 0, -12);
            state.EntityManager.SetComponentData(pEntity, new LocalTransform { Position = pPos, Scale = 1 });
            //���Ѫ��
            Day6GameRoot.ins.AddHpToEntity(state.EntityManager, pEntity, pPos);
            state.EntityManager.AddComponent<Day6PlayerTag>(pEntity);
            flag = 1;
            ChangeWeponType(state.EntityManager, pEntity, 1);
        }
        using (EntityCommandBuffer ecb=new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        {
            //���ҵ����
            foreach (var (pTran, ept, php, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Day6PlayerTag>, Day6Hp>().WithEntityAccess())
            {
                h = Input.GetAxis("Horizontal");
                if (h != 0)
                {
                    pTran.ValueRW.Position += new Unity.Mathematics.float3(h, 0, 0) * SystemAPI.Time.DeltaTime * 10;
                }
                pTran.ValueRW.Position = math.clamp(pTran.ValueRW.Position, new float3(-24, 0, -12), new float3(24, 0, -12));
                //Ѫ������
                php.sli.transform.position = Camera.main.WorldToScreenPoint(pTran.ValueRW.Position + (float3)Vector3.up);
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    changeFlag++;
                    if (changeFlag % 2 == 0)
                    {
                        ChangeWeponType(state.EntityManager, pEntity, 1);
                    }
                    else
                    {
                        ChangeWeponType(state.EntityManager, pEntity, 2);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //�����ӵ�
                    Entity bEntity = default;
                    if (changeFlag % 2 == 0)
                    {
                        bEntity = state.EntityManager.Instantiate(data.biu);
                        //����ӵ����
                       ecb.AddComponent(bEntity, new Day6BiuData { livedTimer = 2, type = 1 });
                        //����λ��
                        state.EntityManager.SetComponentData(bEntity, new LocalTransform { Position = pTran.ValueRW.Position, Scale = 0.5f });
                    }
                    else
                    {
                        bEntity = state.EntityManager.Instantiate(data.bar);
                        //����ӵ����
                        ecb.AddComponent(bEntity, new Day6BiuData { livedTimer = 2, type = 0 });
                        //����λ��
                        state.EntityManager.SetComponentData(bEntity, new LocalTransform { Position = pTran.ValueRW.Position, Scale = 1f });
                    }
                   
                }
            }
            ecb.Playback(state.EntityManager);
        }
      
    }
    private void ChangeWeponType(EntityManager em, Entity entity, int index)
    {
        //�ж��Ƿ��и��ӹ�ϵ
        if (em.HasBuffer<LinkedEntityGroup>(entity))
        {
            //��ȡ������
            var linkGroup = em.GetBuffer<LinkedEntityGroup>(entity);
            for (global::System.Int32 i = 0; i < linkGroup.Length; i++)
            {
                em.SetComponentEnabled<MaterialMeshInfo>(linkGroup[i].Value, false);
            }
            em.SetComponentEnabled<MaterialMeshInfo>(linkGroup[0].Value, true);
            em.SetComponentEnabled<MaterialMeshInfo>(linkGroup[index].Value, true);
        }
    }
}
