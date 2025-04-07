using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 敌人系统
/// </summary>
public partial struct Day8_9EnemySystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day8_9Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //查找到敌人
            foreach (var (eTran,ed,ehp,eEntity) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day8_9EnemyData>,Day8_9HpData>().WithEntityAccess())
            {
                //首先判断是否有主城
                if (Day8_9GameRoot.ins.hasMainCity)
                {
                    //判断是否到达目标位置
                    if (math.distance(eTran.ValueRW.Position,ed.ValueRW.targetPos)>0.5f)
                    {
                        //计算移动方向
                        var dir = ed.ValueRW.targetPos - eTran.ValueRW.Position;
                        eTran.ValueRW.Position += dir*SystemAPI.Time.DeltaTime*0.1f;
                        //判断是否触发了瞭望塔的警告时间
                        if (math.abs(eTran.ValueRW.Position.x-8)<1)
                        {
                            Day8_9GameRoot.ins.canShowViewTime = true;
                        }
                    }
                    //血条跟随
                    ehp.sli.transform.position = Camera.main.WorldToScreenPoint(eTran.ValueRW.Position+new float3(0,1,0));
                }
                //查找到主城
                foreach (var (bTran,bd,bhp,bEntity) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day8_9BuildingData>,Day8_9HpData>().WithEntityAccess())
                {
                    //首先判断是主城
                    if (bd.ValueRW.bType== Day8_9BuilingType.MainCity)
                    {
                        //判断敌人和主城的位置
                        if (math.distance(eTran.ValueRW.Position, bTran.ValueRW.Position) < 4)
                        {
                            ed.ValueRW.attackTime -= SystemAPI.Time.DeltaTime;
                            if (ed.ValueRW.attackTime <= 0)
                            {
                                bhp.sli.value -= 1;
                                if (bhp.sli.value<=0)
                                {
                                    ecb.DestroyEntity(bEntity);
                                    Time.timeScale = 0;
                                }
                                ed.ValueRW.attackTime = 2;
                            }
                        }
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
    } 
}
