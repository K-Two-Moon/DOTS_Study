using Unity.Entities;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// �����ƶ�
/// </summary>
public partial struct Day6EnemySystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day6Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb=new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        {
            //���ҵ����еĵ���
            foreach (var (et, ehp, ed, eEntity) in SystemAPI.Query<RefRO<Day6EnemyTag>, Day6Hp, Day6AnimObj>().WithEntityAccess())
            {
                //���Ŵ�������
                SystemAPI.ManagedAPI.GetComponent<Animator>(eEntity).SetBool("Is_move", true);
                //�ƶ�
                ed.ins.position += ed.ins.transform.forward * SystemAPI.Time.DeltaTime * 0.5f;
                //Ѫ������
                ehp.sli.transform.position = Camera.main.WorldToScreenPoint(ed.ins.position + Vector3.up);
                if (ed.ins.position.z < -15)//���߽�
                {
                    ecb.DestroyEntity(eEntity);
                }
            }
            ecb.Playback(state.EntityManager);
        }
     
    }
}
