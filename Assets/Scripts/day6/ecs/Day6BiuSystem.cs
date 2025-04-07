using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// �ӵ�ϵͳ
/// </summary>
public partial struct Day6BiuSystem : ISystem
{
    private Day6InsData data;
    private Unity.Mathematics.Random rnd;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day6Tag>();
        rnd=new Unity.Mathematics.Random(state.GlobalSystemVersion);
    }
    void OnUpdate(ref SystemState state)
    {
        //�������
        foreach (var item in SystemAPI.Query<RefRO<Day6InsData>>())
        {
            data = item.ValueRO;
        }
        using (EntityCommandBuffer ecb=new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        {
            //���ҵ����е��ӵ�
            foreach (var (bTran,bd,bEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day6BiuData>>().WithEntityAccess())
            {
                bTran.ValueRW.Position += bTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 15;
                //���ҵ����еĵ���
                foreach (var (et, ehp, ed, eEntity) in SystemAPI.Query<RefRO<Day6EnemyTag>, Day6Hp, Day6AnimObj>().WithEntityAccess())
                {
                    if (bd.ValueRW.type == 1)//��ͨ�ӵ�
                    {
                        if (math.distance(bTran.ValueRW.Position, ed.ins.position)<0.5f)
                        {
                            ecb.DestroyEntity(bEntity);
                            ehp.sli.value -= 40;
                            if (ehp.sli.value<=0)
                            {
                                ecb.DestroyEntity(eEntity);
                                Day6GameRoot.ins.killCount++;
                                Day6GameRoot.ins.killCountDes.text = $"��ɱ������{Day6GameRoot.ins.killCount}";
                                Day6GameRoot.ins.socreCount+=100;
                                Day6GameRoot.ins.scoreDes.text = $"�÷֣�{Day6GameRoot.ins.socreCount}";
                                DropProp(state.EntityManager,ecb);
                            }
                        }
                    }
                    if (bd.ValueRW.type == 0)//bar
                    {
                        //�жϵ��˺�bar�ľ���
                        if (math.abs(bTran.ValueRW.Position.z- ed.ins.position.z)<=1)
                        {
                            ecb.DestroyEntity(eEntity);
                            Day6GameRoot.ins.killCount++;
                            Day6GameRoot.ins.killCountDes.text = $"��ɱ������{Day6GameRoot.ins.killCount}";
                            Day6GameRoot.ins.socreCount += 100;
                            Day6GameRoot.ins.scoreDes.text = $"�÷֣�{Day6GameRoot.ins.socreCount}";
                            DropProp(state.EntityManager,ecb);
                        }
                        if (bTran.ValueRW.Position.z >= 15)
                        {
                            ecb.DestroyEntity(bEntity);
                        }
                    }
                }
                bd.ValueRW.livedTimer -= SystemAPI.Time.DeltaTime;
                if (bd.ValueRW.livedTimer <= 0)
                {
                    ecb.DestroyEntity(bEntity);
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }

    private void DropProp(EntityManager em,EntityCommandBuffer ecb)
    {
        if (Day6GameRoot.ins.killCount%6==0)
        {
            var pEntity = em.Instantiate(data.prop);
            var rndPos = rnd.NextFloat3(new float3(-24, 0, -12), new float3(24, 0, -12));
            em.SetComponentData(pEntity, new LocalTransform { Position=rndPos, Scale=1 });
            //��ӵ����������
            ecb.AddComponent(pEntity, new Day6PropData { livedTimer=5, maxScale=3 });
        }
    }
}
