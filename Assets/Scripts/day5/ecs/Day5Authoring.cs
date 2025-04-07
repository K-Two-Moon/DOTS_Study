using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
/// </summary>
public class Day5Authoring : MonoBehaviour
{
    public GameObject t1, t2;

    class Day5Baker : Baker<Day5Authoring>
    {
        public override void Bake(Day5Authoring authoring)
        {
            //获取实体
            var entity=GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day5Tag>(entity);
            //添加组件
            AddComponent(entity, new Day5InsData { 
             type1=GetEntity(authoring.t1, TransformUsageFlags.Dynamic),
             type2=GetEntity(authoring.t2, TransformUsageFlags.Dynamic),
            });
        }
    }
}
