using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
/// <summary>
/// 敌人实体，负责敌人的相关逻辑处理 
/// </summary>
public partial struct Day2EnemySystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day2Tag>();  //该系统下面的OnUpdate函数只有存在该标签的时候才会进行更新
    }

    void OnUpdate(ref SystemState state)
    {
        //查找到所有的敌人
        foreach (var (eTran, ed, ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2EnemyData>, Day2Hp>().WithEntityAccess())
        {   //血条跟随
            ehp.sli.transform.position = Camera.main.WorldToScreenPoint(eTran.ValueRW.Position + new float3(0, 1, 0));
        }
    }
}
