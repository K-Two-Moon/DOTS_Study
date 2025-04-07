using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
/// </summary>
public class Day14Authoring : MonoBehaviour
{
    public GameObject player;
    class Day14Baker : Baker<Day14Authoring>
    {
        public override void Bake(Day14Authoring authoring)
        {
            //获取实体
            var entity=GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day14Tag>(entity);
            //添加组件
            AddComponent(entity, new Day14InsData {  player=GetEntity(authoring.player, TransformUsageFlags.Dynamic)});
        }
    }
}
