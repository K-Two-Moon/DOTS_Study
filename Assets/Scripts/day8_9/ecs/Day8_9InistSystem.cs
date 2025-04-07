using System;
using System.Xml;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 初始化系统
/// </summary>
public partial struct Day8_9InistSystem : ISystem
{

    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day8_9Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        //查找组件
        foreach (var item in SystemAPI.Query<RefRW<Day8_9InsData>>())
        {
            Day8_9GameRoot.ins.data = item.ValueRO;
            Day8_9GameRoot.ins.em = state.EntityManager;
        }
        //查找到所有的拖拽的ui
        var dragLst = GameObject.Find("Canvas").transform.Find("drag").GetComponentsInChildren<Button>();
        for (int i = 0; i < dragLst.Length; i++)
        {
            DragMgr.Get(dragLst[i].gameObject).bindOnBeginDrag += Day8_9InistSystem_bindOnBeginDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnDrag += Day8_9InistSystem_bindOnDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnEndDrag += Day8_9InistSystem_bindOnEndDrag;
        }

        //显示生成主城(大本营)
        var mEntity = state.EntityManager.Instantiate(Day8_9GameRoot.ins.data.maincity);
        var mPos = new Vector3(-16, 0, -16);
        state.EntityManager.SetComponentData(mEntity, new LocalTransform { Position = mPos, Scale = 0.5f });
        AddBulidngHub(state.EntityManager, mEntity, mPos, "主城");
        state.EntityManager.AddComponentData(mEntity, new Day8_9BuildingData { bType = Day8_9BuilingType.MainCity });

        //矿场
        var kEntity = state.EntityManager.Instantiate(Day8_9GameRoot.ins.data.mine);
        var kPos = new Vector3(-20, 0, 0);
        state.EntityManager.SetComponentData(kEntity, new LocalTransform { Position = kPos, Rotation = Quaternion.AngleAxis(180, Vector3.up), Scale = 0.5f });
        //生成敌人
        CreateEnemes(mPos);
    }

    private void CreateEnemes(Vector3 mPos)
    {
        Vector3 pos = new Vector3(19,0.001f,-10);
        //随机敌人
        int count = UnityEngine.Random.Range(5, 8);
        for (int i = 0; i < count; i++)
        {
            var eEntity = Day8_9GameRoot.ins.em.Instantiate(Day8_9GameRoot.ins.data.enemy);
            //
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            var realPos = pos + new Vector3(rndPos.x, 0, rndPos.y);
            var targetPos= mPos + new Vector3(rndPos.x, 0, rndPos.y); 
            Day8_9GameRoot.ins.em.SetComponentData(eEntity, new LocalTransform { Position = realPos, Scale = 1 });
            Day8_9GameRoot.ins.em.AddComponentData(eEntity, new Day8_9EnemyData { attackTime=2,  hp=10, targetPos= targetPos });
            //添加血条
            Day8_9GameRoot.ins.AddHpToEntity(Day8_9GameRoot.ins.em, eEntity, realPos);
        }
    }

    private void Day8_9InistSystem_bindOnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            Day8_9GameRoot.ins.dragUiName = eventData.pointerEnter.gameObject.name;
            var entity = GetInsEntity();
            if (entity != default)
            {
                Day8_9GameRoot.ins.dragEntity = Day8_9GameRoot.ins.em.Instantiate(entity);
                //添加组件
                var name = GetName();
                Day8_9GameRoot.ins.em.AddComponentData(Day8_9GameRoot.ins.dragEntity, new Day8_9BuildingData { name = name });
                Day8_9GameRoot.ins.em.SetComponentData(Day8_9GameRoot.ins.dragEntity, new LocalTransform { Scale = 0.5f });
            }
        }
    }

    private void Day8_9InistSystem_bindOnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //首先判断拖拽对实体是否存在
        if (Day8_9GameRoot.ins.dragEntity != default)
        {
            //判断拖拽的实体身上是否有变换组件
            if (Day8_9GameRoot.ins.em.HasComponent<LocalTransform>(Day8_9GameRoot.ins.dragEntity))
            {
                //获取拖拽实体身上的变换组件
                var tran = Day8_9GameRoot.ins.em.GetComponentData<LocalTransform>(Day8_9GameRoot.ins.dragEntity);
                //计算坐标
                var vpos = Camera.main.WorldToScreenPoint(tran.Position);
                //
                var wpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vpos.z));
                tran.Position = wpos;
                //复制给拖拽的实体
                Day8_9GameRoot.ins.em.SetComponentData(Day8_9GameRoot.ins.dragEntity, tran);
            }
        }
    }

    private void Day8_9InistSystem_bindOnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //判断
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            //判断是否可以放置的区域
            if (eventData.pointerCurrentRaycast.gameObject.name.CompareTo("place") == 0&& Day8_9GameRoot.ins.dragEntity!=default)
            {
                var pos = eventData.pointerCurrentRaycast.worldPosition;
                Day8_9GameRoot.ins.em.SetComponentData(Day8_9GameRoot.ins.dragEntity, new LocalTransform { Position = pos, Scale = 0.5f });
                var name = GetName();
                AddBulidngHub(Day8_9GameRoot.ins.em, Day8_9GameRoot.ins.dragEntity, pos, name);
                switch (Day8_9GameRoot.ins.dragUiName)
                {
                    case "barrack":
                        CreateSoldier(pos);
                        break;
                    case "maincity":
                        //当拖拽的是主城时主城不存在
                        if (Day8_9GameRoot.ins.hasMainCity && Day8_9GameRoot.ins.dragUiName == "maincity")
                        {
                            Day8_9GameRoot.ins.em.DestroyEntity(Day8_9GameRoot.ins.dragEntity);
                        }
                        break;
                    case "tower":
                        NotifyTower();
                        break;
                    case "tec":
                        UpSoldierLevel();
                        break;
                }
            }
            else
            {
                Day8_9GameRoot.ins.em.DestroyEntity(Day8_9GameRoot.ins.dragEntity);
            }
        }
        Day8_9GameRoot.ins.dragEntity = default;
    }

    private void NotifyTower()
    {
         
    }

    private void UpSoldierLevel()
    {
        //构建查询器
        var query = new EntityQueryBuilder(Unity.Collections.Allocator.Temp).WithAll<Day8_9SoldierData>().Build(Day8_9GameRoot.ins.em);
        var array = query.ToEntityArray(Unity.Collections.Allocator.Temp);
        for (int i = 0; i < array.Length; i++)
        {
            var tran = Day8_9GameRoot.ins.em.GetComponentData<LocalTransform>(array[i]);
            tran.Scale += 0.1f;
            Day8_9GameRoot.ins.em.SetComponentData(array[i], tran);
        }
    }

    private void CreateSoldier(Vector3 pos)
    {
        for (int i = 0; i < 5; i++)
        {
            var sEntity = Day8_9GameRoot.ins.em.Instantiate(Day8_9GameRoot.ins.data.soldier);
            //
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            var realPos = pos + new Vector3(rndPos.x,0, rndPos.y);
            Day8_9GameRoot.ins.em.SetComponentData(sEntity, new LocalTransform {  Position=realPos,Scale=1});
            Day8_9GameRoot.ins.em.AddComponentData(sEntity, new Day8_9SoldierData { attackTime=1 });
            //添加血条
            Day8_9GameRoot.ins.AddHpToEntity(Day8_9GameRoot.ins.em, sEntity, realPos);
        }
    }

    private string GetName()
    {
        string name = "";
        switch (Day8_9GameRoot.ins.dragUiName)
        {
            case "mine":
                name = "矿场";
                break;
            case "barrack":
                name = "兵工厂";
                break;
            case "maincity":
                name = "主城";
                break;
            case "tower":
                name = "瞭望塔";
                break;
            case "tec":
                name = "科技营";
                break;
        }
        return name;
    }


    /// <summary>
    /// 根据拖拽的ui的名称获取需要实例化的实体
    /// </summary>
    private Entity GetInsEntity()
    {
        Entity e = default;
        switch (Day8_9GameRoot.ins.dragUiName)
        {
            case "mine":
                e = Day8_9GameRoot.ins.data.mine;
                break;
            case "barrack":
                e = Day8_9GameRoot.ins.data.barrack;
                break;
            case "maincity":
                e = Day8_9GameRoot.ins.data.maincity;
                break;
            case "tower":
                e = Day8_9GameRoot.ins.data.tower;
                break;
            case "tec":
                e = Day8_9GameRoot.ins.data.tec;
                break;
        }
        return e;
    }

    private void AddBulidngHub(EntityManager em, Entity entity, Vector3 pos, string msg)
    {
        //生成血条
        var sliTran = GameObject.Instantiate(Resources.Load<GameObject>("newhp"), Day8_9GameRoot.ins.transform).transform;
        sliTran.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
        var sli = sliTran.GetComponent<Slider>();
        sliTran.Find("neck").GetComponent<Text>().text = msg;
        em.AddComponentObject(entity, new Day8_9HpData { sli = sli });
    }
}
