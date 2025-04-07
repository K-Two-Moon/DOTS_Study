using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 子弹的系统，负责子弹的逻辑处理
/// </summary>
public partial struct Day2BiuSystem : ISystem
{
    private Day2InsData data;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day2Tag>();  //该系统下面的OnUpdate函数只有存在该标签的时候才会进行更新
    }

    void OnUpdate(ref SystemState state)
    {
        //查找实例化组件
        foreach (var item in SystemAPI.Query<RefRO<Day2InsData>>())
        {
            data = item.ValueRO;
        }

        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp))
        {
            //查找到子弹
            foreach (var (bTran, bd, bEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2BiuData>>().WithEntityAccess())
            {
                //子弹移动
                bTran.ValueRW.Position += bTran.ValueRW.Forward() * SystemAPI.Time.DeltaTime * bd.ValueRW.moveSpeed;
                //
                bd.ValueRW.livedTimer -= SystemAPI.Time.DeltaTime;
                if (bd.ValueRW.livedTimer <= 0)
                {
                    //state.EntityManager.DestroyEntity(bEntity);
                    ecb.DestroyEntity(bEntity);
                }
                //查找到所有的敌人
                foreach (var (eTran, ed,ehp, eEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2EnemyData>,Day2Hp>().WithEntityAccess())
                {
                    //判断子弹和敌人的距离
                    if (math.distance(bTran.ValueRW.Position, eTran.ValueRW.Position) < 0.5f)
                    {
                        //子弹消失
                        ecb.DestroyEntity(bEntity);
                        #region  方法1
                        ////todo:生成一个掉落物品
                        //var dropEntity = ecb.Instantiate(data.drop);
                        //ecb.SetComponent(dropEntity, new LocalTransform { Position = eTran.ValueRW.Position, Scale = 1 });
                        ////添加掉落物品组件
                        ////ecb.AddComponent(dropEntity, new Day2DropTag { });
                        //ecb.AddComponent<Day2DropTag>(dropEntity);
                        //敌人消失
                        //ecb.DestroyEntity(eEntity);
                        //Day2Helper.eCount--;
                        #endregion

                        #region 扩展
                        
                        ed.ValueRW.hp -= 40;
                        //血条更新
                        ehp.sli.value=ed.ValueRW.hp;
                        if (ed.ValueRW.hp <= 0)
                        {
                            //todo:生成一个掉落物品
                            var dropEntity = ecb.Instantiate(data.drop);
                            ecb.SetComponent(dropEntity, new LocalTransform { Position = eTran.ValueRW.Position, Scale = 1 });
                            //添加掉落物品组件
                            //ecb.AddComponent(dropEntity, new Day2DropTag { });
                            ecb.AddComponent<Day2DropTag>(dropEntity);
                            //敌人消失
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
