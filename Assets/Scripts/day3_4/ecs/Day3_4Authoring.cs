using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
/// </summary>
public class Day3_4Authoring : MonoBehaviour
{
    public GameObject player, enemey;
    class Day3_4Baker : Baker<Day3_4Authoring>
    {
        public override void Bake(Day3_4Authoring authoring)
        {
            //获取实体
            var entity = GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day3_4Tag>(entity);
            //添加组件
            AddComponent(entity, new Day3_4InsData
            {
                player = GetEntity(authoring.player, TransformUsageFlags.Dynamic),
                enemy = GetEntity(authoring.enemey, TransformUsageFlags.Dynamic),
            });
        }
    }
}
