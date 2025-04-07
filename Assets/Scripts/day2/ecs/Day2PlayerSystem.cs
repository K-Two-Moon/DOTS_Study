using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// 定义玩家系统，负责玩家的相关逻辑
/// </summary>
public partial struct Day2PlayerSystem : ISystem
{
    private Day2InsData data;
    private float h, v;
    private float3 playerPos;
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
            /*
       * pTran:接受查找到的LocalTransform组件
       * pd:接受查找到的Day2PlayerData组件
       * pEntity：接受实体身上同时拥有LocalTransform和Day2PlayerData组件的实体
       */
            //查找到玩家
            foreach (var (pTran, pd,php, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2PlayerData>,Day2Hp>().WithEntityAccess())
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                if (h != 0 || v != 0)
                {
                    //玩家移动
                    pTran.ValueRW.Position += new Unity.Mathematics.float3(h, 0, v) * SystemAPI.Time.DeltaTime * pd.ValueRW.moveSpeed;
                    //摄像机跟随
                    Camera.main.transform.position = pTran.ValueRW.Position + new Unity.Mathematics.float3(0, 2, -4);
                }
                playerPos = pTran.ValueRW.Position;
                //血条跟随
                php.sli.transform.position = Camera.main.WorldToScreenPoint(pTran.ValueRW.Position+new float3(0,1,0));
                //查找到掉落物品
                foreach (var (dTran, dt, dEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day2DropTag>>().WithEntityAccess())
                {
                    //判断距离
                    if (math.distance(pTran.ValueRW.Position, dTran.ValueRW.Position) < 1f)
                    {
                        //销毁掉落物品
                        ecb.DestroyEntity(dEntity);
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }
      
        //按下空格发射子弹
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //生成子弹实体
            var bEntity = state.EntityManager.Instantiate(data.biu);
            //设置子弹的位置
            state.EntityManager.SetComponentData(bEntity, new LocalTransform {  Position=playerPos, Scale=0.5f});
            //为子弹添加组件
            state.EntityManager.AddComponentData(bEntity, new Day2BiuData {  livedTimer=2, moveSpeed=10});
        }
    }
}
