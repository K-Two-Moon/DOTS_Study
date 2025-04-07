using Unity.Entities;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 敌人移动
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
            //查找到所有的敌人
            foreach (var (et, ehp, ed, eEntity) in SystemAPI.Query<RefRO<Day6EnemyTag>, Day6Hp, Day6AnimObj>().WithEntityAccess())
            {
                //播放待机动画
                SystemAPI.ManagedAPI.GetComponent<Animator>(eEntity).SetBool("Is_move", true);
                //移动
                ed.ins.position += ed.ins.transform.forward * SystemAPI.Time.DeltaTime * 0.5f;
                //血条跟随
                ehp.sli.transform.position = Camera.main.WorldToScreenPoint(ed.ins.position + Vector3.up);
                if (ed.ins.position.z < -15)//到边界
                {
                    ecb.DestroyEntity(eEntity);
                }
            }
            ecb.Playback(state.EntityManager);
        }
     
    }
}
