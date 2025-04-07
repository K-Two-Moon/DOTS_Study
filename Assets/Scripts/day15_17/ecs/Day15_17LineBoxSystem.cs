using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 划线系统
/// </summary>
public partial class Day15_17LineBoxSystem : SystemBase
{
    float Minx, Minz, Maxx, Maxz;
    Vector3 v0, v1, v2, v3;
    LineRenderer Lr;
    private bool canDraw;
    protected override void OnCreate()
    {
        RequireForUpdate<Day15_17Tag>();
    }
    protected override void OnUpdate()
    {
        if (Lr==null)
        {
             Lr =GameObject.Find("lr").GetComponent<LineRenderer>();
        }
        if (Input.GetMouseButtonDown(1))
        {
            //判断ui事件检测到的游戏对象不为null
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                //当前选中的对象不是建造按钮，则可以进行拖拽移动
                if (EventSystem.current.currentSelectedGameObject.tag == "uitag")
                {
                    Debug.Log("|-----------|");
                    return;
                }
            }
            v0 = GetPos(Input.mousePosition);
            canDraw = true;
           
        }
        if (Input.GetMouseButton(1))
        {   
            v2 = GetPos(Input.mousePosition);
        }

        if (canDraw)
        {
            Lr.positionCount = 4;
            v3 = new Vector3(v0.x, 0, v2.z);
            v1 = new Vector3(v2.x, 0, v0.z);
            Lr.SetPosition(0, v0);
            Lr.SetPosition(1, v1);
            Lr.SetPosition(2, v2);
            Lr.SetPosition(3, v3);
        }
        if (Input.GetMouseButtonUp(1))
        {         
            var eqb = new EntityQueryBuilder(Allocator.Temp).WithAll<Day15_17SoldierData>().Build(EntityManager);
            var array = eqb.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < array.Length; i++)
            {
                var Tran = EntityManager.GetComponentData<LocalTransform>(array[i]);
                GetPos();           
                if (Tran.Position.x>=Minx&&Tran.Position.z>=Minz&&Tran.Position.x<=Maxx&&Tran.Position.z<=Maxz)
                {
                    EntityManager.AddComponentData(array[i], new Day15_17MoveData {  speed=0.35f});
                }
            }
            canDraw = false;
            Lr.positionCount = 0;
        }
        //右键移动
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var (sd,md) in SystemAPI.Query<RefRW<Day15_17SoldierData>,RefRW<Day15_17MoveData>>())
            {
                //设置可以移动的标记
                md.ValueRW.selectFlag = 1;
                sd.ValueRW.targetPos = GetRndPos();
            }
        }
        using (EntityCommandBuffer ecb=new EntityCommandBuffer( Allocator.Temp))
        {
            //查找到然后进行移动
            foreach (var (sTran, sd, md, ehp,sEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17SoldierData>, RefRW<Day15_17MoveData>, Day15_17Hp>().WithEntityAccess())
            {
                if (math.distance(sTran.ValueRW.Position, sd.ValueRW.targetPos) > 0.5f)
                {
                    //计算方向
                    var dir = sd.ValueRW.targetPos - sTran.ValueRW.Position;
                    sTran.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * md.ValueRW.speed;
                    ehp.sli.transform.position = Camera.main.WorldToScreenPoint((Vector3)sTran.ValueRW.Position + Vector3.up);
                }
                else
                {
                    Debug.Log("1111");

                    if (md.ValueRW.selectFlag==1)
                    {
                        Debug.Log("2222");
                        ecb.RemoveComponent<Day15_17MoveData>(sEntity);
                    }
                }
            }
            ecb.Playback(EntityManager);
        }
    }

    private Vector3 GetRndPos()
    {
        Vector3 targetpos=new Vector3();
        UnityEngine.Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            //设置随机位置
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            targetpos = hit.point + new Vector3(rndPos.x, 0, rndPos.y);
        }
        return  targetpos;
    }

    private void GetPos()
    {
        Minx = Mathf.Min(v0.x, v2.x);
        Minz = Mathf.Min(v0.z, v2.z);
        Maxz = Mathf.Max(v0.z, v2.z);
        Maxx = Mathf.Max(v0.x, v2.x);
    }
    private Vector3 GetPos(Vector3 mousePosition)
    {
        mousePosition.z = 25;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
