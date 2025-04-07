using System;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 初始化系统
/// </summary>
public partial struct Day15_17InitSystem : ISystem
{
    //注意：这里的成员变量只能是私有的值类型
    private Unity.Mathematics.Random rnd;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day15_17Tag>();
        rnd = new Unity.Mathematics.Random(state.GlobalSystemVersion);
    }
    void OnUpdate(ref SystemState state)
    {
        //确保该OnUpdate函数只调用一次
        state.Enabled = false;
        //进行实例化组件数据的查询
        foreach (var item in SystemAPI.Query<RefRW<Day15_17InsData>>())
        {
            //记录
            Day15_17GameRoot.ins.em = state.EntityManager;
            Day15_17GameRoot.ins.insData = item.ValueRO;
        }
        //进行事件的拖拽绑定
        for (int i = 0; i < Day15_17GameRoot.ins.dragLst.Count; i++)
        {
            DragMgr.Get(Day15_17GameRoot.ins.dragLst[i]).bindOnBeginDrag += Day15_17InitSystem_bindOnBeginDrag;
            DragMgr.Get(Day15_17GameRoot.ins.dragLst[i]).bindOnDrag += Day15_17InitSystem_bindOnDrag;
            DragMgr.Get(Day15_17GameRoot.ins.dragLst[i]).bindOnEndDrag += Day15_17InitSystem_bindOnEndDrag;
        }
        //生成玩家
        var pEntity = Day15_17GameRoot.ins.em.Instantiate(Day15_17GameRoot.ins.insData.player);
        var pPos = new Vector3(0, 0, -10);
        Day15_17GameRoot.ins.em.SetComponentData(pEntity, new LocalTransform { Position = pPos, Scale = 1 });
        Day15_17GameRoot.ins.em.AddComponentData(pEntity, new Day15_17PlayerTag { });
        Day15_17GameRoot.ins.AddHpToEnity(Day15_17GameRoot.ins.em, pEntity, pPos);
        //修改玩家的初始生命值
       var hp= Day15_17GameRoot.ins.em.GetComponentData<Day15_17Hp>(pEntity);
        hp.sli.value = 50;
        Day15_17GameRoot.ins.em.SetComponentData(pEntity, hp);
        //创建8个小怪
        CreateEnemies();
    }

    private void CreateEnemies()
    {
        var pos = new Vector3(19, 0.0001f, -11.4f);
        for (int i = 0; i < 8; i++)
        {
            //生成敌人实体
            var entity = Day15_17GameRoot.ins.em.Instantiate(Day15_17GameRoot.ins.insData.enemy);
            //设置随机位置
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            var realPos = pos + new Vector3(rndPos.x, 0, rndPos.y);
            Day15_17GameRoot.ins.em.SetComponentData(entity, new LocalTransform { Position = realPos, Scale = 1 });
            //添加敌人组件
            Day15_17GameRoot.ins.em.AddComponentData(entity, new Day15_17EnemyData { attackTimer = 2, attackValue = 5, hp = 100, targetPos = realPos });
            //为敌人添加血条
            Day15_17GameRoot.ins.AddHpToEnity(Day15_17GameRoot.ins.em, entity, realPos);
            Day15_17GameRoot.ins.eCount++;
            Day15_17GameRoot.ins.UpdateEnemyCount();
        }
    }

    private void Day15_17InitSystem_bindOnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            Day15_17GameRoot.ins.uiName = eventData.pointerEnter.gameObject.name;
            var entity = GetInsEntity();
            if (entity != default)
            {
                Day15_17GameRoot.ins.dragEntity = Day15_17GameRoot.ins.em.Instantiate(entity);
                var data = GetInsBData(Day15_17GameRoot.ins.uiName);//根据名称获取配置数据
                Day15_17GameRoot.ins.tmpData = data;
                Day15_17GameRoot.ins.em.SetComponentData(Day15_17GameRoot.ins.dragEntity, new LocalTransform { Scale = data.size });
                Day15_17GameRoot.ins.em.AddComponentData(Day15_17GameRoot.ins.dragEntity, new Day15_17BuildingData { bName = data.name, hp = data.hp });
            }
        }
    }

    private void Day15_17InitSystem_bindOnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //判断拖拽的实体是否存在
        if (Day15_17GameRoot.ins.dragEntity != default)
        {
            //判断拖拽的实体身上是否有变化组件
            if (Day15_17GameRoot.ins.em.HasComponent<LocalTransform>(Day15_17GameRoot.ins.dragEntity))
            {
                //获取拖拽实体身上的变换组件
                var tran = Day15_17GameRoot.ins.em.GetComponentData<LocalTransform>(Day15_17GameRoot.ins.dragEntity);
                //计算
                var vPos = Camera.main.WorldToScreenPoint(tran.Position);
                //
                var wPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vPos.z));
                //赋值
                tran.Position = wPos;
                Day15_17GameRoot.ins.em.SetComponentData(Day15_17GameRoot.ins.dragEntity, tran);
            }
        }
    }

    private void Day15_17InitSystem_bindOnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && Day15_17GameRoot.ins.dragEntity != default)
        {
            //注意主摄像机身上需要有PhysicsRaycaster组件
            if (eventData.pointerCurrentRaycast.gameObject.tag == "place")//判断射线检测到的游戏对象是可以放置的标志
            {
                var pos = eventData.pointerCurrentRaycast.worldPosition;
                var tran = Day15_17GameRoot.ins.em.GetComponentData<LocalTransform>(Day15_17GameRoot.ins.dragEntity);
                tran.Position = pos;
                Day15_17GameRoot.ins.em.SetComponentData(Day15_17GameRoot.ins.dragEntity, tran);
                Day15_17GameRoot.ins.AddHpToEnity(Day15_17GameRoot.ins.em, Day15_17GameRoot.ins.dragEntity, pos, Day15_17GameRoot.ins.tmpData.name);
                switch (Day15_17GameRoot.ins.uiName)
                {
                    case "tower":

                        break;
                    case "barrack":
                         CreateSoldier(pos);
                        break;
                    case "maincity":
                        NotifyEnemiesMainCityPlaceOver(pos);
                        break;
                }
            }
            else
            {
                Day15_17GameRoot.ins.em.DestroyEntity(Day15_17GameRoot.ins.dragEntity);
            }
        }
        Day15_17GameRoot.ins.dragEntity = default;
    }

    /// <summary>
    /// 通知敌人，主城的位置
    /// </summary>
    private void NotifyEnemiesMainCityPlaceOver(Vector3 pos)
    {
        //构建eqb实体查询器
        var eqb = new EntityQueryBuilder(Unity.Collections.Allocator.Temp).WithAll<Day15_17EnemyData>().Build(Day15_17GameRoot.ins.em);
        var array = eqb.ToEntityArray(Unity.Collections.Allocator.Temp);
        for (int i = 0; i < array.Length; i++)
        {
            var data = Day15_17GameRoot.ins.em.GetComponentData<Day15_17EnemyData>(array[i]);
            //设置随机位置
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            var targetPos = pos + new Vector3(rndPos.x, 0, rndPos.y);
            data.targetPos = targetPos;
            Day15_17GameRoot.ins.em.SetComponentData(array[i], data);
        }
    }

    private void CreateSoldier(Vector3 pos)
    {

        for (int i = 0; i < 5; i++)
        {
            //生成小兵实体
            var entity = Day15_17GameRoot.ins.em.Instantiate(Day15_17GameRoot.ins.insData.soldier);
            //设置随机位置
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            var realPos = pos + new Vector3(rndPos.x, 0, rndPos.y);

            //改变颜色
            if (i % 2 == 0)
            {
                var data = Day15_17GameRoot.ins.eDataLst[0];
                Day15_17GameRoot.ins.em.SetComponentData(entity, new LocalTransform { Position = realPos, Scale = data.size });
                Day15_17GameRoot.ins.em.SetComponentData(entity, new URPMaterialPropertyBaseColor { Value = new Unity.Mathematics.float4(data.r, data.g, data.b, data.a) });
            }
            else
            {
                var data = Day15_17GameRoot.ins.eDataLst[1];
                Day15_17GameRoot.ins.em.SetComponentData(entity, new LocalTransform { Position = realPos, Scale = data.size });
                Day15_17GameRoot.ins.em.SetComponentData(entity, new URPMaterialPropertyBaseColor { Value = new Unity.Mathematics.float4(data.r, data.g, data.b, data.a) });
            }
            //为小兵添加血条
            Day15_17GameRoot.ins.AddHpToEnity(Day15_17GameRoot.ins.em, entity, realPos);

            //添加士兵组件
            Day15_17GameRoot.ins.em.AddComponentData(entity, new Day15_17SoldierData { hp = 100, attackTimer = 1, attackValue = 20, targetPos = realPos });
            Day15_17GameRoot.ins.sCount++;
            Day15_17GameRoot.ins.UpdateSoldierCount();
        }
    }

    /// <summary>
    /// 根据名称获取需要进行实例化的实体
    /// </summary>

    private Entity GetInsEntity()
    {
        Entity en = default;
        var data = GetInsBData(Day15_17GameRoot.ins.uiName);
        if (data != null)//配置数据不能为null
        {
            if (!string.IsNullOrEmpty(data.bType))//不为空
            {
                switch (data.bType)
                {
                    case "tower":
                        en = Day15_17GameRoot.ins.insData.tower;
                        break;
                    case "barrack":
                        en = Day15_17GameRoot.ins.insData.barrack;
                        break;
                    case "maincity":
                        en = Day15_17GameRoot.ins.insData.maincity;
                        break;
                }
            }
        }
        return en;
    }

    /// <summary>
    /// 根据拖拽的UI，从配置数据中获取数据
    /// </summary>
    private BDataEntity GetInsBData(string pName)
    {
        BDataEntity bDataEntity = null;
        for (int i = 0; i < Day15_17GameRoot.ins.bDataLst.Count; i++)
        {
            if (Day15_17GameRoot.ins.bDataLst[i].bType == pName)
            {
                bDataEntity = Day15_17GameRoot.ins.bDataLst[i];
                break;
            }
        }
        return bDataEntity;
    }
}
