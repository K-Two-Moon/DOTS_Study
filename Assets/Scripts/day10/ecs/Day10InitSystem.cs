using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 初始化系统
/// </summary>
public partial struct Day10InitSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day10Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        //该更新函数只调用一次
        state.Enabled = false;
        //查找组件
        foreach (var item in SystemAPI.Query<RefRW<Day10InsData>>())
        {
            //记录
            Day10GameRoot.ins.em = state.EntityManager;
            Day10GameRoot.ins.insData = item.ValueRO;
        }
        //查找到ui
        var dragLst = GameObject.Find("Canvas").transform.Find("drag").GetComponentsInChildren<Button>();
        //进行ui按钮的拖拽绑定
        for (int i = 0; i < dragLst.Length; i++)
        {
            DragMgr.Get(dragLst[i].gameObject).bindOnBeginDrag += Day10InitSystem_bindOnBeginDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnDrag += Day10InitSystem_bindOnDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnEndDrag += Day10InitSystem_bindOnEndDrag;
        }
    }

    private void Day10InitSystem_bindOnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //判断是否为null
        if (eventData.pointerEnter != null)
        {
            //记录拖拽的ui的名称
            Day10GameRoot.ins.dragUIName = eventData.pointerEnter.name;
            //根据拖拽的名称获取对应的需要进行实例化的实体
            var entity = GetInsEntity();
            if (entity!=default)
            {  //实例化实体
                Day10GameRoot.ins.dragEntity = Day10GameRoot.ins.em.Instantiate(entity);
                //togo:根据需要添加组件数据
                Day10GameRoot.ins.em.AddComponentData(Day10GameRoot.ins.dragEntity, new Day10BuildingData {  des = Day10GameRoot.ins.dragUIName });
            }
        }
    }

    private Entity GetInsEntity()
    {
        Entity ins = default;
        switch (Day10GameRoot.ins.dragUIName)
        {
            case "barrack":
                ins = Day10GameRoot.ins.insData.barrack;
                break;
            case "mine":
                ins = Day10GameRoot.ins.insData.mine;
                break;
        }
        return ins;
    }

    private void Day10InitSystem_bindOnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //判断拖拽的实体是否存在
        if (Day10GameRoot.ins.dragEntity != default)
        {
            //判断拖拽的实体身上是否有变换组件
            if (Day10GameRoot.ins.em.HasComponent<LocalTransform>(Day10GameRoot.ins.dragEntity))
            {
                //获取拖拽的实体身上的变换组件
                var tran = Day10GameRoot.ins.em.GetComponentData<LocalTransform>(Day10GameRoot.ins.dragEntity);
                //计算
                var vPos = Camera.main.WorldToScreenPoint(tran.Position);
                //vpos.z和鼠标的位置，重新计算出来新的坐标
                var wPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vPos.z));
                tran.Position = wPos;
                //重新作用于实体
                Day10GameRoot.ins.em.SetComponentData(Day10GameRoot.ins.dragEntity, tran);
            }
        }
    }

    private void Day10InitSystem_bindOnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //判断是否为空，同时拖拽的实体是否存在
        if (eventData.pointerCurrentRaycast.gameObject != null && Day10GameRoot.ins.dragEntity != default)
        {
            var pos = eventData.pointerCurrentRaycast.worldPosition;


            //测试
            var col = Day10GameRoot.ins.GetSize(pos, 0.5f);
            if (Day10GameRoot.ins.HasOwn(col))
            {
                Debug.Log("弹出有格子被占用");
            }

            for (global::System.Int32 i = 0; i < col.Length; i++)
            {
                col[i].gameObject.name = "1";
            }
           
            //判断是否是可以放置的区域
            //if (eventData.pointerCurrentRaycast.gameObject.name == "place")
            //{
              
            //    Day10GameRoot.ins.em.SetComponentData(Day10GameRoot.ins.dragEntity, new LocalTransform { Position = pos, Scale = 1 });
              
            //}
            //else
            //{
            //    //销毁
            //    Day10GameRoot.ins.em.DestroyEntity(Day10GameRoot.ins.dragEntity);
            //}
        }
        Day10GameRoot.ins.dragEntity = default;//可以理解为清空
    }
}
