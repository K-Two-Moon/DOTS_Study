using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// ����ϵͳ
/// </summary>
public partial struct Day6PropSystem : ISystem
{
    /// <summary>
    /// ��¼��ʱ��ק��ʵ��
    /// </summary>
    private Entity dragEntity;
    /// <summary>
    /// ��קʵ���Ĭ�ϱ任���
    /// </summary>
    private LocalTransform defafultTran;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day6Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //���ҵ�������Ʒ
            foreach (var (pTran, upd, pd, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day6UsePropData>, RefRW<Day6PropData>>().WithEntityAccess())
            {
                pd.ValueRW.livedTimer -= SystemAPI.Time.DeltaTime;
                if (pd.ValueRW.livedTimer <= 0)
                {
                    ecb.DestroyEntity(pEntity);
                }
                pTran.ValueRW.Scale += SystemAPI.Time.DeltaTime;
                if (pTran.ValueRW.Scale > pd.ValueRW.maxScale)
                {
                    //���ҵ����еĵ���
                    foreach (var (et, ehp, ed, eEntity) in SystemAPI.Query<RefRO<Day6EnemyTag>, Day6Hp, Day6AnimObj>().WithEntityAccess())
                    {
                        if (math.distance(pTran.ValueRW.Position, ed.ins.transform.position) < 5)
                        {
                            ecb.DestroyEntity(eEntity);
                        }
                    }
                    ecb.DestroyEntity(pEntity);
                }
            }
            ecb.Playback(state.EntityManager);
        }
        //
        if (Input.GetMouseButtonDown(0))
        {
            //��������
            var hit = GetECSHit();
            //�ж����߼���ʵ���Ƿ��ǵ���
            if (state.EntityManager.HasComponent<Day6PropData>(hit.Entity))
            {
                dragEntity = hit.Entity;//��¼��ק��ʵ��
                defafultTran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
            }
        }
        if (Input.GetMouseButton(0))
        {
            //�����ж���ק��ʵ���Ƿ����
            if (dragEntity != default)
            {
                //�ж���ק��ʵ�������Ƿ��б任���
                if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
                {
                    Debug.Log("hit2");
                    //��ȡ��קʵ�����ϵı任���
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                    //����ת��
                    var vpos = Camera.main.WorldToScreenPoint(tran.Position);
                    //���λ�ý��vpos��z���¼������һ���µ���������
                    var wpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vpos.z));
                    tran.Position = wpos;
                    //Ӧ�õ�ʵ��
                    state.EntityManager.SetComponentData(dragEntity, tran);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //�ж���ק��ʵ�������Ƿ��б任���
            if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
            {
                //��ȡ��קʵ�����ϵı任���
                var tran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                if (tran.Position.z < -11)
                {
                    state.EntityManager.SetComponentData(dragEntity, defafultTran);
                }
                else
                {
                    //���ʹ�ñ�ǩ
                    state.EntityManager.AddComponent<Day6UsePropData>(dragEntity);
                }
            }
            dragEntity = default;
        }
    }

    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. 
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. 
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput() { Start = ray.origin, End = ray.direction * 1000, Filter = CollisionFilter.Default };
        //3. 
        Unity.Physics.RaycastHit hit;
        //4. 
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. 
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit;
    }
    private UnityEngine.RaycastHit GetHit()
    {
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        UnityEngine.RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

        }
        return hit;
    }
}
