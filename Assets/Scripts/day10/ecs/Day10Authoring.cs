using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Day10Authoring : MonoBehaviour
{
    public GameObject barrack, mine, soldier;
    class Day10Baker : Baker<Day10Authoring>
    {
        public override void Bake(Day10Authoring authoring)
        {
            //��ȡʵ��
            var entity = GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day10Tag>(entity);
            //������
            AddComponent(entity, new Day10InsData
            {
                barrack = GetEntity(authoring.barrack, TransformUsageFlags.Dynamic),
                mine = GetEntity(authoring.mine, TransformUsageFlags.Dynamic),
                soldier = GetEntity(authoring.soldier, TransformUsageFlags.Dynamic),
            });
        }
    }
}
