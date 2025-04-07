using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
//�﷨��
using Random = Unity.Mathematics.Random;

/// <summary>
/// ��ʼ��ϵͳ�������ʼ����ص�ҵ���߼�
/// </summary>
public partial struct Day2InitSystem : ISystem
{
    private Day2InsData data;
    private  Random rnd;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day2Tag>();  //��ϵͳ�����OnUpdate����ֻ�д��ڸñ�ǩ��ʱ��Ż���и���
        rnd = new Unity.Mathematics.Random(state.GlobalSystemVersion);
    }

    void OnUpdate(ref SystemState state)
    {
        Day2Helper.GameOver(false);
        state.Enabled = false;//�ø��º���ֻ����һ�� 
        //����ʵ�������
        foreach (var item in SystemAPI.Query<RefRO<Day2InsData>>())
        {
            data = item.ValueRO;
        }
        //���ɼ�������
        for (int i = 0; i < 5; i++)
        {
            //���ɵ���ʵ��
            var eEntity = state.EntityManager.Instantiate(data.enemy);
            //���õ��˵����λ��
            var rndPos = UnityEngine.Random.insideUnitCircle * 10;
            var realPos = new Vector3(0,0.5f,0) + new Vector3(rndPos.x,0,rndPos.y);
            state.EntityManager.SetComponentData(eEntity, new LocalTransform { Position=realPos, Scale=1});
            //���������ɫ
            state.EntityManager.SetComponentData(eEntity, new URPMaterialPropertyBaseColor {  Value=rnd.NextFloat4()});
            //Ϊ���˶�̬��ӵ����������
            state.EntityManager.AddComponentData(eEntity,new Day2EnemyData{ attackTimer=2, hp=100, attacValue=5, targetPos=realPos  });
            Day2Helper.eCount++;
            //ΪС�����Ѫ��
            Day2Helper.AddHpToEntity(state.EntityManager,eEntity,realPos);
        }
        //�������
        var playerEntity=state.EntityManager.Instantiate(data.player);
        var pPos = new Vector3(0, 0.5f, -8);
        //�������λ��
        state.EntityManager.SetComponentData(playerEntity, new LocalTransform { Position = pPos, Scale = 1 });
        //Ϊ��������ҵ��������
        state.EntityManager.AddComponentData(playerEntity, new Day2PlayerData {  hp=100, moveSpeed=5 });
        //Ϊ������Ѫ��
        Day2Helper.AddHpToEntity(state.EntityManager, playerEntity, pPos);
        //���������
        Camera.main.transform.position = pPos + new Vector3(0, 2, -4);
    }
}
