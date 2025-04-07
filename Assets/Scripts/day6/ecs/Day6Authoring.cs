using Unity.Entities;
using UnityEngine;
/// <summary>
/// 创作数据
/// </summary>
public class Day6Authoring : MonoBehaviour
{
    public GameObject enemy, player, biu, bar, prop;
    class Day6Baker : Baker<Day6Authoring>
    {
        public override void Bake(Day6Authoring authoring)
        {

            //获取实体
            var entity = GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day6Tag>(entity);
            //添加组件
            AddComponent(entity, new Day6InsData
            {
                enemy = GetEntity(authoring.enemy, TransformUsageFlags.Dynamic),
                player = GetEntity(authoring.player, TransformUsageFlags.Dynamic),
                biu = GetEntity(authoring.biu, TransformUsageFlags.Dynamic),
                bar = GetEntity(authoring.bar, TransformUsageFlags.Dynamic),
                prop = GetEntity(authoring.prop, TransformUsageFlags.Dynamic),
            });
        }
    }
}
