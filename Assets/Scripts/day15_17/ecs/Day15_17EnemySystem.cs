using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 敌人系统
/// </summary>
public partial struct Day15_17EnemySystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day15_17Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        //查找到敌人
        foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17EnemyData>, Day15_17Hp>().WithEntityAccess())
        {
            //计算移动方向
            var dir = ed.ValueRW.targetPos - eTran.ValueRW.Position;
            //是否到达了目标位置
            if (math.distance(eTran.ValueRW.Position, ed.ValueRW.targetPos) > 0.5f)
            {
                eTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * 0.2f;
                //血条跟随
                ehp.sli.transform.position=Camera.main.WorldToScreenPoint((Vector3)eTran.ValueRW.Position+Vector3.up);
            }
            else
            {
                //到达目标位置开始攻击主城
                //查找到主城
                foreach (var (bTran,bd,bhp,bEntity) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Day15_17BuildingData>,Day15_17Hp>().WithEntityAccess())
                {
                    //判断是否是主城
                    if (bd.ValueRW.bName=="主城")
                    {
                        ed.ValueRW.attackTimer -= SystemAPI.Time.DeltaTime;
                        if (ed.ValueRW.attackTimer <= 0)
                        {
                            bhp.sli.value -= ed.ValueRW.attackValue;
                            if (bhp.sli.value<=0)
                            {
                                Debug.Log("游戏结束");
                                Time.timeScale = 0;
                            }
                            ed.ValueRW.attackTimer = 2;
                        }
                    }
                }
            }
        }
    }
}
