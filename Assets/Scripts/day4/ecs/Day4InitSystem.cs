using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using static UnityEditor.Progress;

/// <summary>
/// ��ʼ��
/// </summary>
public partial struct Day4InitSystem : ISystem
{
    private Unity.Mathematics.Random rnd;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day4Tag>();
        rnd = new Unity.Mathematics.Random(state.GlobalSystemVersion);
    }
    void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        using (EntityCommandBuffer ecb=new EntityCommandBuffer( Unity.Collections.Allocator.Temp))
        {
            //���һ������
            foreach (var item in SystemAPI.Query<DynamicBuffer<Day4BufferData>>())
            {
                for (global::System.Int32 i = 0; i < item.Length; i++)
                {
                    //���λ��
                    //var rndPos = rnd.NextFloat3();
                    var rndPos = rnd.NextFloat3(new Unity.Mathematics.float3(-9, 0.5f, -1), new Unity.Mathematics.float3(9, 0.5f, 10));
                    //����ʵ�� 
                    var entity = state.EntityManager.Instantiate(item[i].entity);
                    state.EntityManager.SetComponentData(entity, new LocalTransform { Position = rndPos, Scale = 1 });
                  ecb.AddComponent(entity, new Day4HpData { hp = 100 });
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}

/// <summary>
/// ϵͳ
/// </summary>
public partial struct Day4System : ISystem
{
    /// <summary>
    /// ��¼��ק��ʵ��
    /// </summary>
    private Entity dragEntity;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day4Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        //���ҵ����еĴ�������ֵ��ʵ��
        foreach (var (dhd,dmm,dEntity) in SystemAPI.Query<RefRW<Day4HpData>,RefRW<MaterialMeshInfo>>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState).WithEntityAccess())
        {
            //���ͨ��������ƿ������������
            if (Input.GetKeyDown(KeyCode.A))
            {
                state.EntityManager.SetComponentEnabled<Day4HpData>(dEntity, false);
                state.EntityManager.SetComponentEnabled<MaterialMeshInfo>(dEntity, false);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                state.EntityManager.SetComponentEnabled<Day4HpData>(dEntity, true);
                state.EntityManager.SetComponentEnabled<MaterialMeshInfo>(dEntity, true);
            }
            //�ж�ʵ��������������Ƿ���
            if (state.EntityManager.IsComponentEnabled<Day4HpData>(dEntity))
            {
                dhd.ValueRW.hp -= SystemAPI.Time.DeltaTime;
            }
          
        }

        //��갴��
        if (Input.GetMouseButtonDown(0))
        {
            var hit = GetECSHit();
            //state.EntityManager.DestroyEntity(hit.Entity);
            dragEntity = hit.Entity;//��¼ѡ�е�ʵ��
        }
        //��갴ס���ɿ�
        if (Input.GetMouseButton(0))
        {
            //�ж���ק��ʵ�������Ƿ���localtransfrom
            if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
            {
                //��ȡ��קʵ��ı任
                var  tran=state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                //����ת��
                var vpos=Camera.main.WorldToScreenPoint(tran.Position);
                //���λ�ú�vpos��zֵ�����¼�����������
                var wpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vpos.z));
                tran.Position = wpos;
                //����ӳ�䵽��ק��ʵ��
                state.EntityManager.SetComponentData(dragEntity, tran); 
            }

        }
        //����ɿ�
        if (Input.GetMouseButtonUp(0))
        {
            dragEntity = default;
        }
    }
    /// <summary>
    /// ecs��������߼��
    /// ע�⣺ת��ecs����Ϸ����Ԥ�����ϱ�������ײ������
    /// </summary>
    /// <returns></returns>
    private Unity.Physics.RaycastHit GetECSHit()
    {
        //1. ��ecs���������
        UnityEngine.Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        //2. ecs�������������
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput() {  Start=ray.origin,End=ray.direction*1000, Filter=CollisionFilter.Default};
        //3. ecs�����������ײ
        Unity.Physics.RaycastHit hit;
        //4. ��ȡ�������絥��
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        //5. ��������
        pw.ValueRO.CastRay(raycastInput,out hit);
        return hit; 
    }
}
