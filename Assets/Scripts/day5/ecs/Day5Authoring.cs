using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Day5Authoring : MonoBehaviour
{
    public GameObject t1, t2;

    class Day5Baker : Baker<Day5Authoring>
    {
        public override void Bake(Day5Authoring authoring)
        {
            //��ȡʵ��
            var entity=GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day5Tag>(entity);
            //������
            AddComponent(entity, new Day5InsData { 
             type1=GetEntity(authoring.t1, TransformUsageFlags.Dynamic),
             type2=GetEntity(authoring.t2, TransformUsageFlags.Dynamic),
            });
        }
    }
}
