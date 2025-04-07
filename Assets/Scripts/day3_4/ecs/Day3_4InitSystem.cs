using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// 初始化系统
/// </summary>
public partial struct Day3_4InitSystem : ISystem
{
    private Day3_4InsData data;
    private Unity.Mathematics.Random rnd;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day3_4Tag>();
        rnd = new Unity.Mathematics.Random(state.GlobalSystemVersion);

    }

    void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        //
        foreach (var item in SystemAPI.Query<RefRO<Day3_4InsData>>())
        {
            data = item.ValueRO;
            Day3_4GameRoot.ins.em = state.EntityManager;
            Day3_4GameRoot.ins.data= data;
            Day3_4GameRoot.ins.rnd= rnd;
        }
        //生成玩家
        var pEntity = state.EntityManager.Instantiate(data.player);
        var pPos = new Vector3(0, 0.5f, -6);
        state.EntityManager.SetComponentData(pEntity, new LocalTransform {  Position= pPos ,Scale=1});
        //添加玩家标签
        state.EntityManager.AddComponent<Day3_4PlayerTag>(pEntity);
        //生成敌人
        for (int i = 0; i < 5; i++)
        {
            var eEntity = state.EntityManager.Instantiate(data.enemy);
            //
            var rndPos = UnityEngine.Random.insideUnitCircle * 20;
            var realPos = new Vector3(rndPos.x, 0.5f, rndPos.y);
            state.EntityManager.SetComponentData(eEntity, new LocalTransform { Position =realPos,Scale=1});
            state.EntityManager.SetComponentData(eEntity, new URPMaterialPropertyBaseColor {  Value=rnd.NextFloat4()});
            //添加敌人标签
            state.EntityManager.AddComponent<Day3_4EnemyTag>(eEntity);
        }
    }
}
