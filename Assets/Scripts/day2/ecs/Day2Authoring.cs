using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Day2Authoring : MonoBehaviour
{
    /// <summary>
    /// ��Ҫ���ݵ�ecs�����������Ϸ����
    /// </summary>
    public GameObject player, enemy, biu,drop;

    class Day2Baker : Baker<Day2Authoring>
    {
        public override void Bake(Day2Authoring authoring)
        {
            //��ȡʵ��
            var entity = GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day2Tag>(entity);
            //������
            AddComponent(entity, new Day2InsData
            {
                player = GetEntity(authoring.player, TransformUsageFlags.Dynamic),
                enemy = GetEntity(authoring.enemy, TransformUsageFlags.Dynamic),
                biu = GetEntity(authoring.biu, TransformUsageFlags.Dynamic),
                drop = GetEntity(authoring.drop, TransformUsageFlags.Dynamic),
            });
        }
    }
}
