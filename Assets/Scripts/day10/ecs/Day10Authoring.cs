using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
/// </summary>
public class Day10Authoring : MonoBehaviour
{
    public GameObject barrack, mine, soldier;
    class Day10Baker : Baker<Day10Authoring>
    {
        public override void Bake(Day10Authoring authoring)
        {
            //获取实体
            var entity = GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day10Tag>(entity);
            //添加组件
            AddComponent(entity, new Day10InsData
            {
                barrack = GetEntity(authoring.barrack, TransformUsageFlags.Dynamic),
                mine = GetEntity(authoring.mine, TransformUsageFlags.Dynamic),
                soldier = GetEntity(authoring.soldier, TransformUsageFlags.Dynamic),
            });
        }
    }
}
