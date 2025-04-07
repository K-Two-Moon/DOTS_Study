using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// �߼���ϵͳ
/// </summary>
public partial struct Day10LogicSystem : ISystem
{
    private float timer;
    /// <summary>
    /// ��ʱ��¼��Ҫ�����ƶ���ʵ��
    /// </summary>
    private Entity moveEntity;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day10Tag>();
    }

    void OnUpdate(ref SystemState state)
    {
        timer += SystemAPI.Time.DeltaTime;
        if (timer>=2)
        {
            Day10GameRoot.ins.dCount += 5;
            Day10GameRoot.ins.diamondDes.text = $"��ʯ��{Day10GameRoot.ins.dCount}";
            timer = 0;
        }
        //�������
        if (Input.GetMouseButtonDown(0))
        {
            var hit = GetECSHit();
            //�жϵ�����Ƿ��ǽ���
            if (state.EntityManager.HasComponent<Day10BuildingData>(hit.Entity))
            {
                //�ж��Ƿ������
                var bdata= state.EntityManager.GetComponentData<Day10BuildingData>(hit.Entity);
                if (bdata.des=="barrack")
                {
                    //��ȡ����ı�������λ��
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //����ui
                    Day10GameRoot.ins.ShowSoldierUI(tran.Position);

                }
            }
            //�ж�ѡ�е��Ƿ���ʿ��
            if (state.EntityManager.HasComponent<Day10SoldierData>(hit.Entity))
            {
                moveEntity = hit.Entity;
            }
        }
        //�Ҽ����
        if (Input.GetMouseButtonDown(1))
        {
            if (moveEntity!=default)
            {
                var hitPos = GetHitPos();
                //by way1
                state.EntityManager.SetComponentData(moveEntity, new Day10SoldierData { targetPos = hitPos, hp = 100 });
                //by way2
                //state.EntityManager.AddComponentData(moveEntity, new Day10MoveData {  targetPos=hitPos});
            }

            moveEntity = default;
        }
    }

    private Vector3 GetHitPos()
    {
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        UnityEngine.RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {

        }
        return hit.point;
    }
    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. ��ecs���������
        UnityEngine.Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs�������������
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput {  Start=ray.origin, End=ray.direction*1000, Filter=CollisionFilter.Default};
        //3. ecs����ļ�����
        Unity.Physics.RaycastHit hit;
        //4. ��ȡ������
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. ���߼��
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit; 
    }
}
