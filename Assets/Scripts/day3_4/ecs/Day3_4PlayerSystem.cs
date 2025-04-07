using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �����������
/// </summary>
readonly partial struct Day3_4PlayerAspect:IAspect
{
    public readonly Entity pEntity;
    public readonly RefRW<LocalTransform> pTran;
    public readonly RefRO<Day3_4PlayerTag> pt;
}

/// <summary>
/// ���ϵͳ
/// </summary>
public partial struct Day3_4PlayerSystem : ISystem
{
    private float h, v;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day3_4Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        //using (EntityCommandBuffer ecb = new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        //{
        //    //���ҵ����
        //    foreach (var (pTran, pt, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Day3_4PlayerTag>>().WithEntityAccess())
        //    {
        //        if (Day3_4GameRoot.ins.toggle.isOn)
        //        {
        //            h += Input.GetAxis("Horizontal") * SystemAPI.Time.DeltaTime * 10;
        //            v = Input.GetAxis("Vertical");
        //            if (h != 0 || v != 0)
        //            {
        //                if (v > 0)
        //                {
        //                    pTran.ValueRW.Position += pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
        //                }
        //                if (v < 0)
        //                {
        //                    pTran.ValueRW.Position -= pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
        //                }
        //                //��ת
        //                pTran.ValueRW.Rotation = quaternion.RotateY(h);
        //                //���������
        //                Camera.main.transform.position = pTran.ValueRW.Position + new float3(0, 2, -6);
        //                var dir = pTran.ValueRW.Position - (float3)Camera.main.transform.position;
        //                Camera.main.transform.rotation = Quaternion.LookRotation(dir);
        //            }
        //        }
        //        //������ҵ�λ��
        //        Day3_4GameRoot.ins.posDes.text = $"���λ�ã�{(Vector3)pTran.ValueRW.Position}";
        //        //���ҵ�����
        //        foreach (var (eTran, et, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day3_4EnemyTag>>().WithEntityAccess())
        //        {
        //            //�жϾ���
        //            if (math.distance(pTran.ValueRW.Position, eTran.ValueRW.Position) < 1f)
        //            {
        //                ecb.DestroyEntity(eEntity);
        //            }
        //        }
        //    }
        //    ecb.Playback(state.EntityManager);
        //}
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //���ҵ����
            foreach (var  pa in SystemAPI.Query<Day3_4PlayerAspect>())
            {
                if (Day3_4GameRoot.ins.toggle.isOn)
                {
                    h += Input.GetAxis("Horizontal") * SystemAPI.Time.DeltaTime * 10;
                    v = Input.GetAxis("Vertical");
                    if (h != 0 || v != 0)
                    {
                        if (v > 0)
                        {
                            pa. pTran.ValueRW.Position += pa.pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
                        }
                        if (v < 0)
                        {
                            pa.pTran.ValueRW.Position -= pa.pTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * 5;
                        }
                        //��ת
                        pa.pTran.ValueRW.Rotation = quaternion.RotateY(h);
                        //���������
                        Camera.main.transform.position = pa.pTran.ValueRW.Position + new float3(0, 2, -6);
                        var dir = pa.pTran.ValueRW.Position - (float3)Camera.main.transform.position;
                        Camera.main.transform.rotation = Quaternion.LookRotation(dir);
                    }
                }
                //������ҵ�λ��
                Day3_4GameRoot.ins.posDes.text = $"���λ�ã�{(Vector3)pa.pTran.ValueRW.Position}";
                //���ҵ�����
                foreach (var (eTran, et, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day3_4EnemyTag>>().WithEntityAccess())
                {
                    //�жϾ���
                    if (math.distance(pa.pTran.ValueRW.Position, eTran.ValueRW.Position) < 1f)
                    {
                        ecb.DestroyEntity(eEntity);
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}
