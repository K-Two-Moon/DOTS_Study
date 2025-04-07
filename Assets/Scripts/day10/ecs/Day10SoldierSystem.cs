using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// ʿ��ϵͳ
/// </summary>
public partial struct Day10SoldierSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day10Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //by way1
            //���ҵ�ʿ��
            foreach (var (sTran, sd, shp, sEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day10SoldierData>, Day10Hp>().WithEntityAccess())
            {
                //�����ƶ�����
                var dir = sd.ValueRO.targetPos - sTran.ValueRW.Position;
                if (math.distance(sd.ValueRO.targetPos, sTran.ValueRW.Position) > 0.5f)
                {
                    sTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * 0.5f;
                }
                shp.sli.transform.position = Camera.main.WorldToScreenPoint(sTran.ValueRW.Position + (float3)Vector3.up);
            }

            //by way2
            //foreach (var (sTran, moveData, shp, sEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day10MoveData>, Day10Hp>().WithEntityAccess())
            //{
            //    //�����ƶ�����
            //    var dir = moveData.ValueRO.targetPos - sTran.ValueRW.Position;
            //    if (math.distance(moveData.ValueRO.targetPos, sTran.ValueRW.Position) > 0.5f)
            //    {
            //        sTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * 0.5f;
            //    }
            //    else
            //    {
            //        //����Ŀ��λ��
            //        ecb.RemoveComponent<Day10MoveData>(sEntity);
            //    }
            //    shp.sli.transform.position = Camera.main.WorldToScreenPoint(sTran.ValueRW.Position + (float3)Vector3.up);
            //}
            ecb.Playback(state.EntityManager);
        }

    }
}
