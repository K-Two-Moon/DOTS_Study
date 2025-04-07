using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

/// <summary>
/// ��ʼ��ϵͳ
/// </summary>
public partial struct Day10InitSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day10Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        //�ø��º���ֻ����һ��
        state.Enabled = false;
        //�������
        foreach (var item in SystemAPI.Query<RefRW<Day10InsData>>())
        {
            //��¼
            Day10GameRoot.ins.em = state.EntityManager;
            Day10GameRoot.ins.insData = item.ValueRO;
        }
        //���ҵ�ui
        var dragLst = GameObject.Find("Canvas").transform.Find("drag").GetComponentsInChildren<Button>();
        //����ui��ť����ק��
        for (int i = 0; i < dragLst.Length; i++)
        {
            DragMgr.Get(dragLst[i].gameObject).bindOnBeginDrag += Day10InitSystem_bindOnBeginDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnDrag += Day10InitSystem_bindOnDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnEndDrag += Day10InitSystem_bindOnEndDrag;
        }
    }

    private void Day10InitSystem_bindOnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //�ж��Ƿ�Ϊnull
        if (eventData.pointerEnter != null)
        {
            //��¼��ק��ui������
            Day10GameRoot.ins.dragUIName = eventData.pointerEnter.name;
            //������ק�����ƻ�ȡ��Ӧ����Ҫ����ʵ������ʵ��
            var entity = GetInsEntity();
            if (entity!=default)
            {  //ʵ����ʵ��
                Day10GameRoot.ins.dragEntity = Day10GameRoot.ins.em.Instantiate(entity);
                //togo:������Ҫ����������
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
        //�ж���ק��ʵ���Ƿ����
        if (Day10GameRoot.ins.dragEntity != default)
        {
            //�ж���ק��ʵ�������Ƿ��б任���
            if (Day10GameRoot.ins.em.HasComponent<LocalTransform>(Day10GameRoot.ins.dragEntity))
            {
                //��ȡ��ק��ʵ�����ϵı任���
                var tran = Day10GameRoot.ins.em.GetComponentData<LocalTransform>(Day10GameRoot.ins.dragEntity);
                //����
                var vPos = Camera.main.WorldToScreenPoint(tran.Position);
                //vpos.z������λ�ã����¼�������µ�����
                var wPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vPos.z));
                tran.Position = wPos;
                //����������ʵ��
                Day10GameRoot.ins.em.SetComponentData(Day10GameRoot.ins.dragEntity, tran);
            }
        }
    }

    private void Day10InitSystem_bindOnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //�ж��Ƿ�Ϊ�գ�ͬʱ��ק��ʵ���Ƿ����
        if (eventData.pointerCurrentRaycast.gameObject != null && Day10GameRoot.ins.dragEntity != default)
        {
            var pos = eventData.pointerCurrentRaycast.worldPosition;


            //����
            var col = Day10GameRoot.ins.GetSize(pos, 0.5f);
            if (Day10GameRoot.ins.HasOwn(col))
            {
                Debug.Log("�����и��ӱ�ռ��");
            }

            for (global::System.Int32 i = 0; i < col.Length; i++)
            {
                col[i].gameObject.name = "1";
            }
           
            //�ж��Ƿ��ǿ��Է��õ�����
            //if (eventData.pointerCurrentRaycast.gameObject.name == "place")
            //{
              
            //    Day10GameRoot.ins.em.SetComponentData(Day10GameRoot.ins.dragEntity, new LocalTransform { Position = pos, Scale = 1 });
              
            //}
            //else
            //{
            //    //����
            //    Day10GameRoot.ins.em.DestroyEntity(Day10GameRoot.ins.dragEntity);
            //}
        }
        Day10GameRoot.ins.dragEntity = default;//�������Ϊ���
    }
}
