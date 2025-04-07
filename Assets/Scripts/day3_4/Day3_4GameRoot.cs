using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;

/// <summary>
/// 游戏入口
/// </summary>
public class Day3_4GameRoot : MonoBehaviour
{
    public Text posDes;
    public InputField input;
    public Toggle toggle;
    public EntityManager em;
    public Day3_4InsData data;
    public Unity.Mathematics.Random rnd;
    public static Day3_4GameRoot ins;
    private void Awake()
    {
        if (ins == null) { ins = this; }
    }
    void Start()
    {
        input.onEndEdit.AddListener(OnChangeCount);
    }

    private void OnChangeCount(string arg0)
    {
        ClearAllEnemies();
        int count=int.Parse(arg0);
        //生成敌人
        for (int i = 0; i < count; i++)
        {
            var eEntity = em.Instantiate(data.enemy);
            //
            var rndPos = UnityEngine.Random.insideUnitCircle * 20;
            var realPos = new Vector3(rndPos.x, 0.5f, rndPos.y);
            em.SetComponentData(eEntity, new LocalTransform { Position = realPos, Scale = 1 });
           em.SetComponentData(eEntity, new URPMaterialPropertyBaseColor { Value = rnd.NextFloat4() });
            //添加敌人标签
           em.AddComponent<Day3_4EnemyTag>(eEntity);
        }
    }
    private void ClearAllEnemies()
    {
        //查找到所有的敌人
        var enemies = new EntityQueryBuilder(Allocator.Temp).WithAll<Day3_4EnemyTag>().Build(em);
        var array = enemies.ToEntityArray(Allocator.Temp);
        for (int i = 0; i < array.Length; i++)
        {
            em.DestroyEntity(array[i]);
        }
    }
}
