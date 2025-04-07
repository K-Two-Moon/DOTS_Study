using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

/// <summary>
/// 逻辑系统
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
            //更新钻石
            Day8_9GameRoot.ins.dCount += 5;
            Day8_9GameRoot.ins.demondDes.text = $"钻石：{Day8_9GameRoot.ins.dCount}";
            dTime = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var hit = GetECSHit();
            //判断点击的是否是建筑
            if (state.EntityManager.HasComponent<Day8_9BuildingData>(hit.Entity))
            {
                //获取点击建筑的建筑组件
                var data = state.EntityManager.GetComponentData<Day8_9BuildingData>(hit.Entity);
                if (data.bType == Day8_9BuilingType.MainCity)//点击的是主城
                {
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //生成小车
                    Day8_9GameRoot.ins.ShowCarUI(tran.Position);
                }
                if (data.name == "兵工厂")//点击的是兵工厂
                {
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //生成小车
                    Day8_9GameRoot.ins.ShowSoldierUI(tran.Position);
                }
            }
            //判断点击的是小兵
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
                //为小兵动态添加组件
                state.EntityManager.AddComponentData(hitSoldierEntity, new Day8_9SoldierMoveData { targetPos = targetPos });
            }
        }
        //触发警告
        if (Day8_9GameRoot.ins.canShowViewTime)
        {
            //查找瞭望塔
            foreach (var (bTran, bd, bhp, bEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day8_9BuildingData>, Day8_9HpData>().WithEntityAccess())
            {
                //首先判断是瞭望塔
                if (bd.ValueRW.name == "瞭望塔")
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
        //1. 根据提供的位置，从该位置向世界空间发射射线
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs里面的射线输入结构体
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput() { Start = ray.origin, End = ray.direction * 1000, Filter = CollisionFilter.Default };
        //3. ecs里面的射线输出
        Unity.Physics.RaycastHit hit;
        //4. 获取物理世界单利
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. 在ecs物理世界中发射射线
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit;
    }
}
