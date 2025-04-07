using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// ����ϵͳ
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
        //�������
        foreach (var item in SystemAPI.Query<RefRO<Day6InsData>>())
        {
            data = item.ValueRO;
        }
        timer += SystemAPI.Time.DeltaTime;
        if (timer >= 3)
        {
            //���ɵ���
            var eEnetity = state.EntityManager.CreateEntity();
            var eTran = GameObject.Instantiate(Resources.Load<GameObject>("newEnemy")).transform;
            state.EntityManager.AddComponentObject(eEnetity, eTran.GetComponent<Transform>());
            state.EntityManager.AddComponentObject(eEnetity, eTran.GetComponent<Animator>());
            state.EntityManager.AddComponentObject(eEnetity, new Day6AnimObj { ins=eTran});
            //����λ��
            var rndPos = Day6GameRoot.ins.spawnLst[UnityEngine.Random.Range(0, Day6GameRoot.ins.spawnLst.Length)].position;
            eTran.position = rndPos;
            //���Ѫ��
            Day6GameRoot.ins.AddHpToEntity(state.EntityManager, eEnetity,rndPos);
            //��ӵ��˱�ǩ
            state.EntityManager.AddComponent<Day6EnemyTag>(eEnetity);
           timer = 0;
        }
    }
}
