using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

/// <summary>
/// 初始化系统
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
        //查找组件
        foreach (var item in SystemAPI.Query<RefRO<Day5InsData>>())
        {
            Day5GameRoot.ins.em = state.EntityManager;
            Day5GameRoot.ins.data = item.ValueRO;
        }
        //查找到拖拽的按钮
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
        //判断是否为null
        if (eventData.pointerEnter!=null)
        {
            //记录拖拽的UI的名称
            Day5GameRoot.ins.dragUIName= eventData.pointerEnter.gameObject.name;
            var entity = GetInsEntity();//根据拖拽名称类型获取需要生成的实体
            if (entity!=default)//判断实体是否存在
            {
                //记录拖拽的实体
                Day5GameRoot.ins.dragEntity = Day5GameRoot.ins.em.Instantiate(entity);
                //为建筑添加标签
                Day5GameRoot.ins.em.AddComponent<Day5BuildingTag>(Day5GameRoot.ins.dragEntity);
               //关闭拖拽实体得道碰撞
               SetEntityColliderState(Day5GameRoot.ins.em, Day5GameRoot.ins.dragEntity, false);
            }
        }
    }

    private void Day5InitSystem_bindOnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //注意制作的预设的坐标位置起始点为（0,0，0）
        //首先判断拖拽的实体是否存在
        if (Day5GameRoot.ins.dragEntity!=default)
        {
            //判断拖拽的实体身上是否有变化组件
            if (Day5GameRoot.ins.em.HasComponent<LocalTransform>(Day5GameRoot.ins.dragEntity))
            {
                //获取变化拖拽实体的变换组件
                var tran = Day5GameRoot.ins.em.GetComponentData<LocalTransform>(Day5GameRoot.ins.dragEntity);
                //坐标转换
                var vpos = Camera.main.WorldToScreenPoint(tran.Position);
                //鼠标位置和vpos的z值重新映射一个世界坐标
                var wpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,vpos.z));
                tran.Position = wpos;
                //映射到实体
                Day5GameRoot.ins.em.SetComponentData(Day5GameRoot.ins.dragEntity,tran);
            }
        }
    }

    private void Day5InitSystem_bindOnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //松开时射线碰撞到了游戏对象
        if (eventData.pointerCurrentRaycast.gameObject!=null&& Day5GameRoot.ins.dragEntity!=default)
        {
            //判断是否是可以放置的区域
            if (eventData.pointerCurrentRaycast.gameObject.name=="p")
            {
                //松开时发射射线
                var hit = GetECSHit();
                if (Day5GameRoot.ins.em.HasComponent<Day5BuildingTag>(hit.Entity))//下方有建筑
                {
                    Day5GameRoot.ins.em.DestroyEntity(Day5GameRoot.ins.dragEntity);
                }
                else
                {
                    //记录拖拽松开时射线碰撞到的坐标
                    var pos = eventData.pointerCurrentRaycast.worldPosition;
                    Day5GameRoot.ins.em.SetComponentData(Day5GameRoot.ins.dragEntity, new LocalTransform { Position = pos, Scale = 1 });
                    //开启拖拽实体得道碰撞
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
    /// 根据拖拽的名称获取需要生成的实体
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
    /// 设置实体碰撞器的状态
    /// </summary>
    /// <param name="em">实体管理器</param>
    /// <param name="entity">设置的实体</param>
    /// <param name="isEnabled">是否启用碰撞</param>
    private void SetEntityColliderState(EntityManager em,Entity entity,bool isEnabled)
    {
        //判断实体身上是否有碰撞
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
    /// ECS里面的射线检测
    /// </summary>
    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. 非ecs里面的射线
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs里面的射线如数
        Unity.Physics.RaycastInput raycastInput = new RaycastInput {  Start=ray.origin,End=ray.direction*1000,Filter=CollisionFilter.Default};
        //3. ecs里面的碰撞检测
        Unity.Physics.RaycastHit hit;
        //4. 获取ecs里面的物理世界单利
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5.发射射线
        pw.ValueRO.CastRay(raycastInput,out hit);
        return hit;
    }
}
