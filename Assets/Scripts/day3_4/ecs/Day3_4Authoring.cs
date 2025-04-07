using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Day3_4Authoring : MonoBehaviour
{
    public GameObject player, enemey;
    class Day3_4Baker : Baker<Day3_4Authoring>
    {
        public override void Bake(Day3_4Authoring authoring)
        {
            //��ȡʵ��
            var entity = GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day3_4Tag>(entity);
            //������
            AddComponent(entity, new Day3_4InsData
            {
                player = GetEntity(authoring.player, TransformUsageFlags.Dynamic),
                enemy = GetEntity(authoring.enemey, TransformUsageFlags.Dynamic),
            });
        }
    }
}
