using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
//语法糖
using Random = Unity.Mathematics.Random;

/// <summary>
/// 初始化系统，负责初始化相关的业务逻辑
/// </summary>
public partial struct Day2InitSystem : ISystem
{
    private Day2InsData data;
    private  Random rnd;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day2Tag>();  //该系统下面的OnUpdate函数只有存在该标签的时候才会进行更新
        rnd = new Unity.Mathematics.Random(state.GlobalSystemVersion);
    }

    void OnUpdate(ref SystemState state)
    {
        Day2Helper.GameOver(false);
        state.Enabled = false;//该更新函数只调用一次 
        //查找实例化组件
        foreach (var item in SystemAPI.Query<RefRO<Day2InsData>>())
        {
            data = item.ValueRO;
        }
        //生成几个敌人
        for (int i = 0; i < 5; i++)
        {
            //生成敌人实体
            var eEntity = state.EntityManager.Instantiate(data.enemy);
            //设置敌人的随机位置
            var rndPos = UnityEngine.Random.insideUnitCircle * 10;
            var realPos = new Vector3(0,0.5f,0) + new Vector3(rndPos.x,0,rndPos.y);
            state.EntityManager.SetComponentData(eEntity, new LocalTransform { Position=realPos, Scale=1});
            //设置随机颜色
            state.EntityManager.SetComponentData(eEntity, new URPMaterialPropertyBaseColor {  Value=rnd.NextFloat4()});
            //为敌人动态添加敌人组件数据
            state.EntityManager.AddComponentData(eEntity,new Day2EnemyData{ attackTimer=2, hp=100, attacValue=5, targetPos=realPos  });
            Day2Helper.eCount++;
            //为小兵添加血条
            Day2Helper.AddHpToEntity(state.EntityManager,eEntity,realPos);
        }
        //生成玩家
        var playerEntity=state.EntityManager.Instantiate(data.player);
        var pPos = new Vector3(0, 0.5f, -8);
        //设置玩家位置
        state.EntityManager.SetComponentData(playerEntity, new LocalTransform { Position = pPos, Scale = 1 });
        //为玩家添加玩家的组件数据
        state.EntityManager.AddComponentData(playerEntity, new Day2PlayerData {  hp=100, moveSpeed=5 });
        //为玩家添加血条
        Day2Helper.AddHpToEntity(state.EntityManager, playerEntity, pPos);
        //摄像机跟随
        Camera.main.transform.position = pPos + new Vector3(0, 2, -4);
    }
}
