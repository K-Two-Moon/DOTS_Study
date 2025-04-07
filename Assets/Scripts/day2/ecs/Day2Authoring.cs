using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
/// </summary>
public class Day2Authoring : MonoBehaviour
{
    /// <summary>
    /// 需要传递到ecs世界里面的游戏对象
    /// </summary>
    public GameObject player, enemy, biu,drop;

    class Day2Baker : Baker<Day2Authoring>
    {
        public override void Bake(Day2Authoring authoring)
        {
            //获取实体
            var entity = GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day2Tag>(entity);
            //添加组件
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
