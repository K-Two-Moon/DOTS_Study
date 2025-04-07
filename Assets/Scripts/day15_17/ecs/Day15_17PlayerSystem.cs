using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���ϵͳ
/// </summary>
public partial struct Day15_17PlayerSystem : ISystem
{
    //ע�⣺����ĳ�Ա����ֻ����˽�е�ֵ����
    private float h, v;
    private Entity dragEntity;
    private float recoverHp;
    private float3 playerPos;
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Day15_17Tag>();
    }
    void OnUpdate(ref SystemState state)
    {
        using (EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp))
        {
            //���ҵ����
            foreach (var (pTran, pt, php, pEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17PlayerTag>, Day15_17Hp>().WithEntityAccess())
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
                if (h != 0 || v != 0)
                {
                    pTran.ValueRW.Position += new Unity.Mathematics.float3(h, 0, v) * SystemAPI.Time.DeltaTime * 10;
                }
                playerPos = pTran.ValueRW.Position;//��¼��ҵ�λ��
                //Ѫ������
                php.sli.transform.position = Camera.main.WorldToScreenPoint((Vector3)pTran.ValueRW.Position + Vector3.up);
                //���ҵ�����
                foreach (var (bTran, bd, bhp, bEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Day15_17BuildingData>, Day15_17Hp>().WithEntityAccess())
                {
                    //�ж��Ƿ�������
                    if (bd.ValueRW.bName == "��Ѫ��")
                    {
                        //�ж��Ƿ񵽴��Ѫ������
                        if (math.distance(pTran.ValueRW.Position,bTran.ValueRW.Position)<2)
                        {
                            recoverHp += SystemAPI.Time.DeltaTime;
                            if (recoverHp>=1)
                            {
                                php.sli.value += 1;
                                recoverHp = 0;
                            }
                        }
                    }
                }
            }
            ecb.Playback(state.EntityManager);
        }

        //��갴��
        if (Input.GetMouseButtonDown(1))
        {

            var hit = GetEscHit();
            //�ж��ǽ���
            if (state.EntityManager.HasComponent<Day15_17BuildingData>(hit.Entity))
            {
                //��ȡ�������
                var data = state.EntityManager.GetComponentData<Day15_17BuildingData>(hit.Entity);
                if (data.bName == "������")
                {
                    //��ȡ��������λ��
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(hit.Entity);
                    //��ʾ�����찴ť
                    Day15_17GameRoot.ins.ShowCreateUI(tran.Position);
                }
            }
        }

        //��갴��
        if (Input.GetMouseButtonDown(0))
        {
            //�ж�ui�¼���⵽����Ϸ����Ϊnull
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                //��ǰѡ�еĶ����ǽ��찴ť������Խ�����ק�ƶ�
                if (EventSystem.current.currentSelectedGameObject.tag == "uitag")
                {
                    Debug.Log("|-----------|");
                    return;
                }
            }
            var hit = GetEscHit();
            //�ж��ǽ���
            if (state.EntityManager.HasComponent<Day15_17BuildingData>(hit.Entity))
            {
                //��ȡ�������
                var data = state.EntityManager.GetComponentData<Day15_17BuildingData>(hit.Entity);
                if (data.bName == "������")
                {
                    Debug.Log("-----------");
                    dragEntity = hit.Entity;
                }
            }

        }
        //��갴ס���ɿ�
        if (Input.GetMouseButton(0))
        {
            //�ж���ק��ʵ���Ƿ����
            if (dragEntity != default)
            {
                //�ж���ק��ʵ�������Ƿ��б仯���
                if (state.EntityManager.HasComponent<LocalTransform>(dragEntity))
                {
                    //��ȡ��קʵ�����ϵı任���
                    var tran = state.EntityManager.GetComponentData<LocalTransform>(dragEntity);
                    //����
                    var vPos = Camera.main.WorldToScreenPoint(tran.Position);
                    //
                    var wPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vPos.z));
                    //��ֵ
                    tran.Position = wPos;
                    state.EntityManager.SetComponentData(dragEntity, tran);
                    //��ȡʵ�����ϵ�Ѫ���й����
                    var hp = state.EntityManager.GetComponentData<Day15_17Hp>(dragEntity);
                    hp.sli.transform.position = Camera.main.WorldToScreenPoint((Vector3)tran.Position + Vector3.up);
                }
            }
        }

        //�ɿ����
        if (Input.GetMouseButtonUp(0))
        {
            dragEntity = default;
        }
        if (Input.GetKeyDown(KeyCode.Space))//�ո������ 
        {
            var en = GetColosetEnemy(state.EntityManager,2);
            state.EntityManager.DestroyEntity(en);
        }
    }

    /// <summary>
    /// ��ȡ����Ĺ�����ʵ��
    /// </summary>
    private Entity GetColosetEnemy(EntityManager em,float attackValue)
    {
        //��¼����ľ���
        float minDistance = float.MaxValue;
        //������˵�����
        int index = 0;
        //��¼���ҵ��������ʵ��
        Entity en = default;
        //����eqbʵ���ѯ��
        var query = new EntityQueryBuilder(Unity.Collections.Allocator.Temp).WithAll<Day15_17EnemyData>().Build(em);
        //ת��Ϊʵ������
        var array = query.ToEntityArray(Unity.Collections.Allocator.Temp);
        for (int i = 0; i < array.Length; i++)
        {
            //��ȡʵ�����ϵı任���
            var tran = em.GetComponentData<LocalTransform>(array[i]);
            //������˺����֮��ľ���
            var dis = math.distance(playerPos,tran.Position);
            if (dis<attackValue)//����ҵĹ�����Χ��
            {
                if (dis < minDistance)
                {
                    minDistance = dis;
                    index = i;
                }
            }
        }
        //ȷ�����鲻Խ��
        if (array.Length>index)
        {
            en= array[index]; 
        }
        return en;
    }
    private Unity.Physics.RaycastHit GetEscHit()
    {
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput { Start = ray.origin, End = ray.direction * 1000, Filter = CollisionFilter.Default };
        Unity.Physics.RaycastHit hit;
        var pw = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>();
        pw.ValueRO.CastRay(raycastInput, out hit);
        return hit;
    }
}
