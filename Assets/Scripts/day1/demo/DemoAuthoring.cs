using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class DemoAuthoring : MonoBehaviour
{
    /// <summary>
    /// �Ѹ���Ϸ����ͨ��bake���ݵ�ecs������
    /// </summary>
    public GameObject cube;
    class DemoBaker : Baker<DemoAuthoring>
    {
        public override void Bake(DemoAuthoring authoring)
        {
             //��ȡʵ��
             var entity=GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ���
            AddComponent<DemoDataTag>(entity);
            //������
            AddComponent(entity, new DemoInsData { 
             insCube=GetEntity(authoring.cube, TransformUsageFlags.Dynamic),
            });
        }
    }
}
