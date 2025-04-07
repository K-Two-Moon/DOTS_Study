using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

/// <summary>
/// ��ʼ��ϵͳ
/// </summary>
public partial struct Day5InitSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day5Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        //�������
        foreach (var item in SystemAPI.Query<RefRO<Day5InsData>>())
        {
            Day5GameRoot.ins.em = state.EntityManager;
            Day5GameRoot.ins.data = item.ValueRO;
        }
        //���ҵ���ק�İ�ť
        var dragLst = GameObject.Find("Canvas").transform.GetComponentsInChildren<Button>();
        for (int i = 0; i < dragLst.Length; i++)
        {
            DragMgr.Get(dragLst[i].gameObject).bindOnBeginDrag += Day5InitSystem_bindOnBeginDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnDrag += Day5InitSystem_bindOnDrag;
            DragMgr.Get(dragLst[i].gameObject).bindOnEndDrag += Day5InitSystem_bindOnEndDrag;
        }
    }

    private void Day5InitSystem_bindOnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //�ж��Ƿ�Ϊnull
        if (eventData.pointerEnter!=null)
        {
            //��¼��ק��UI������
            Day5GameRoot.ins.dragUIName= eventData.pointerEnter.gameObject.name;
            var entity = GetInsEntity();//������ק�������ͻ�ȡ��Ҫ���ɵ�ʵ��
            if (entity!=default)//�ж�ʵ���Ƿ����
            {
                //��¼��ק��ʵ��
                Day5GameRoot.ins.dragEntity = Day5GameRoot.ins.em.Instantiate(entity);
                //Ϊ������ӱ�ǩ
                Day5GameRoot.ins.em.AddComponent<Day5BuildingTag>(Day5GameRoot.ins.dragEntity);
               //�ر���קʵ��õ���ײ
               SetEntityColliderState(Day5GameRoot.ins.em, Day5GameRoot.ins.dragEntity, false);
            }
        }
    }

    private void Day5InitSystem_bindOnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //ע��������Ԥ�������λ����ʼ��Ϊ��0,0��0��
        //�����ж���ק��ʵ���Ƿ����
        if (Day5GameRoot.ins.dragEntity!=default)
        {
            //�ж���ק��ʵ�������Ƿ��б仯���
            if (Day5GameRoot.ins.em.HasComponent<LocalTransform>(Day5GameRoot.ins.dragEntity))
            {
                //��ȡ�仯��קʵ��ı任���
                var tran = Day5GameRoot.ins.em.GetComponentData<LocalTransform>(Day5GameRoot.ins.dragEntity);
                //����ת��
                var vpos = Camera.main.WorldToScreenPoint(tran.Position);
                //���λ�ú�vpos��zֵ����ӳ��һ����������
                var wpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,vpos.z));
                tran.Position = wpos;
                //ӳ�䵽ʵ��
                Day5GameRoot.ins.em.SetComponentData(Day5GameRoot.ins.dragEntity,tran);
            }
        }
    }

    private void Day5InitSystem_bindOnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //�ɿ�ʱ������ײ������Ϸ����
        if (eventData.pointerCurrentRaycast.gameObject!=null&& Day5GameRoot.ins.dragEntity!=default)
        {
            //�ж��Ƿ��ǿ��Է��õ�����
            if (eventData.pointerCurrentRaycast.gameObject.name=="p")
            {
                //�ɿ�ʱ��������
                var hit = GetECSHit();
                if (Day5GameRoot.ins.em.HasComponent<Day5BuildingTag>(hit.Entity))//�·��н���
                {
                    Day5GameRoot.ins.em.DestroyEntity(Day5GameRoot.ins.dragEntity);
                }
                else
                {
                    //��¼��ק�ɿ�ʱ������ײ��������
                    var pos = eventData.pointerCurrentRaycast.worldPosition;
                    Day5GameRoot.ins.em.SetComponentData(Day5GameRoot.ins.dragEntity, new LocalTransform { Position = pos, Scale = 1 });
                    //������קʵ��õ���ײ
                    SetEntityColliderState(Day5GameRoot.ins.em, Day5GameRoot.ins.dragEntity, true);
                }
            }
            else
            {
                Day5GameRoot.ins.em.DestroyEntity(Day5GameRoot.ins.dragEntity);
            }
        }
        Day5GameRoot.ins.dragUIName = "";
        Day5GameRoot.ins.dragEntity = default;
    }

    /// <summary>
    /// ������ק�����ƻ�ȡ��Ҫ���ɵ�ʵ��
    /// </summary>
    private Entity GetInsEntity()
    {
        Entity en = default;
        switch (Day5GameRoot.ins.dragUIName)
        {
            case "t1":
                en = Day5GameRoot.ins.data.type1;
                break;
            case "t2":
                en = Day5GameRoot.ins.data.type2;
                break;
        }
        return en;
    }

    /// <summary>
    /// ����ʵ����ײ����״̬
    /// </summary>
    /// <param name="em">ʵ�������</param>
    /// <param name="entity">���õ�ʵ��</param>
    /// <param name="isEnabled">�Ƿ�������ײ</param>
    private void SetEntityColliderState(EntityManager em,Entity entity,bool isEnabled)
    {
        //�ж�ʵ�������Ƿ�����ײ
        if (em.HasComponent<PhysicsWorldIndex>(entity))
        {
            if (isEnabled)
            {
                em.SetSharedComponentManaged<PhysicsWorldIndex>(entity, new PhysicsWorldIndex { Value=0});
            }
            else
            {
                em.SetSharedComponentManaged<PhysicsWorldIndex>(entity, new PhysicsWorldIndex { Value = 1 });
            }
        }
    }

    /// <summary>
    /// ECS��������߼��
    /// </summary>
    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. ��ecs���������
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs�������������
        Unity.Physics.RaycastInput raycastInput = new RaycastInput {  Start=ray.origin,End=ray.direction*1000,Filter=CollisionFilter.Default};
        //3. ecs�������ײ���
        Unity.Physics.RaycastHit hit;
        //4. ��ȡecs������������絥��
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5.��������
        pw.ValueRO.CastRay(raycastInput,out hit);
        return hit;
    }
}
