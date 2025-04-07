using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// �ӵ���ϵͳ�������ӵ����߼�����
/// </summary>
public partial struct Day2BiuSystem : ISystem
{
    private Day2InsData data;
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
            //���ҵ��ӵ�
            foreach (var (bTran, bd, bEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2BiuData>>().WithEntityAccess())
            {
                //�ӵ��ƶ�
                bTran.ValueRW.Position += bTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * bd.ValueRW.moveSpeed;
                //
                bd.ValueRW.livedTimer -= SystemAPI.Time.DeltaTime;
                if (bd.ValueRW.livedTimer <= 0)
                {
                    //state.EntityManager.DestroyEntity(bEntity);
                    ecb.DestroyEntity(bEntity);
                }
                //���ҵ����еĵ���
                foreach (var (eTran, ed,ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2EnemyData>,Day2Hp>().WithEntityAccess())
                {
                    //�ж��ӵ��͵��˵ľ���
                    if (math.distance(bTran.ValueRW.Position, eTran.ValueRW.Position) < 0.5f)
                    {
                        //�ӵ���ʧ
                        ecb.DestroyEntity(bEntity);
                        #region  ����1
                        ////todo:����һ��������Ʒ
                        //var dropEntity = ecb.Instantiate(data.drop);
                        //ecb.SetComponent(dropEntity, new LocalTransform { Position = eTran.ValueRW.Position, Scale = 1 });
                        ////��ӵ�����Ʒ���
                        ////ecb.AddComponent(dropEntity, new Day2DropTag { });
                        //ecb.AddComponent<Day2DropTag>(dropEntity);
                        //������ʧ
                        //ecb.DestroyEntity(eEntity);
                        //Day2Helper.eCount--;
                        #endregion

                        #region ��չ
                        
                        ed.ValueRW.hp -= 40;
                        //Ѫ������
                        ehp.sli.value=ed.ValueRW.hp;
                        if (ed.ValueRW.hp <= 0)
                        {
                            //todo:����һ��������Ʒ
                            var dropEntity = ecb.Instantiate(data.drop);
                            ecb.SetComponent(dropEntity, new LocalTransform { Position = eTran.ValueRW.Position, Scale = 1 });
                            //��ӵ�����Ʒ���
                            //ecb.AddComponent(dropEntity, new Day2DropTag { });
                            ecb.AddComponent<Day2DropTag>(dropEntity);
                            //������ʧ
                            ecb.DestroyEntity(eEntity);
                            Day2Helper.eCount--;
                        }
                        #endregion

                      
                        if (Day2Helper.eCount == 0)
                        {
                            Day2Helper.GameOver(true);
                        }
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}
