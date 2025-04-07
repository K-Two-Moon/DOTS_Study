using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// 敌人系统
/// </summary>
public partial struct Day6SpawnEnemySystem : ISystem
{
    private Day6InsData data;
    private float timer;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day6Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        //查找组件
        foreach (var item in SystemAPI.Query<RefRO<Day6InsData>>())
        {
            data = item.ValueRO;
        }
        timer += SystemAPI.Time.DeltaTime;
        if (timer >= 3)
        {
            //生成敌人
            var eEnetity = state.EntityManager.CreateEntity();
            var eTran = GameObject.Instantiate(Resources.Load<GameObject>("newEnemy")).transform;
            state.EntityManager.AddComponentObject(eEnetity, eTran.GetComponent<Transform>());
            state.EntityManager.AddComponentObject(eEnetity, eTran.GetComponent<Animator>());
            state.EntityManager.AddComponentObject(eEnetity, new Day6AnimObj { ins=eTran});
            //设置位置
            var rndPos = Day6GameRoot.ins.spawnLst[UnityEngine.Random.Range(0, Day6GameRoot.ins.spawnLst.Length)].position;
            eTran.position = rndPos;
            //添加血条
            Day6GameRoot.ins.AddHpToEntity(state.EntityManager, eEnetity,rndPos);
            //添加敌人标签
            state.EntityManager.AddComponent<Day6EnemyTag>(eEnetity);
           timer = 0;
        }
    }
}
