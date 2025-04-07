using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
/// </summary>
public class DemoAuthoring : MonoBehaviour
{
    /// <summary>
    /// 把该游戏对象，通过bake传递到ecs世界中
    /// </summary>
    public GameObject cube;
    class DemoBaker : Baker<DemoAuthoring>
    {
        public override void Bake(DemoAuthoring authoring)
        {
             //获取实体
             var entity=GetEntity(TransformUsageFlags.None);
            //添加标签组件
            AddComponent<DemoDataTag>(entity);
            //添加组件
            AddComponent(entity, new DemoInsData { 
             insCube=GetEntity(authoring.cube, TransformUsageFlags.Dynamic),
            });
        }
    }
}
