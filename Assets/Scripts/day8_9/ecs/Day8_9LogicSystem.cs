using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

/// <summary>
/// �߼�ϵͳ
/// </summary>
public partial struct Day8_9LogicSystem : ISystem
{
    private float dTime;
    private Entity hitSoldierEntity;
    private float ttimer;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day8_9Tag>();
        ttimer = 28*0.05f;
    }
    void OnUpdate(ref SystemState state)
    {
        dTime += SystemAPI.Time.DeltaTime;
        if (dTime >= 2)
        {
            //������ʯ
            Day8_9GameRoot.ins.dCount += 5;
            Day8_9GameRoot.ins.demondDes.text = $"��ʯ��{Day8_9GameRoot.ins.dCount}";
            dTime = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var hit = GetECSHit();
            //�жϵ�����Ƿ��ǽ���
            if (state.EntityManager.HasComponent<Day8_9BuildingData>(hit.Entity))
            {
                //��ȡ��������Ľ������
                var data = state.EntityManager.GetComponentData<Day8_9BuildingData>(hit.Entity);
                if (data.bType == Day8_9BuilingType.MainCity)//�����������
                {
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //����С��
                    Day8_9GameRoot.ins.ShowCarUI(tran.Position);
                }
                if (data.name == "������")//������Ǳ�����
                {
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //����С��
                    Day8_9GameRoot.ins.ShowSoldierUI(tran.Position);
                }
            }
            //�жϵ������С��
            if (state.EntityManager.HasComponent<Day8_9SoldierData>(hit.Entity))
            {
                hitSoldierEntity = hit.Entity;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (hitSoldierEntity != default)
            {
                var targetPos = GetPos();
                //ΪС����̬������
                state.EntityManager.AddComponentData(hitSoldierEntity, new Day8_9SoldierMoveData { targetPos = targetPos });
            }
        }
        //��������
        if (Day8_9GameRoot.ins.canShowViewTime)
        {
            //���Ҳt����
            foreach (var (bTran, bd, bhp, bEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day8_9BuildingData>, Day8_9HpData>().WithEntityAccess())
            {
                //�����ж��ǲt����
                if (bd.ValueRW.name == "�t����")
                {
                    ttimer -= SystemAPI.Time.DeltaTime;
                    var tdes = bhp.sli.transform.Find("time").GetComponent<Text>();
                    if (ttimer > 0)
                    {
                        tdes.text = Mathf.CeilToInt(ttimer).ToString();
                    }
                    else
                    {
                        tdes.text = "";
                    }
                }
            }
        }
       
    }

    private Vector3 GetPos()
    {
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        UnityEngine.RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

        }
        return hit.point;
    }

    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. �����ṩ��λ�ã��Ӹ�λ��������ռ䷢������
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs�������������ṹ��
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput() { Start = ray.origin, End = ray.direction * 1000, Filter = CollisionFilter.Default };
        //3. ecs������������
        Unity.Physics.RaycastHit hit;
        //4. ��ȡ�������絥��
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. ��ecs���������з�������
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit;
    }
}
