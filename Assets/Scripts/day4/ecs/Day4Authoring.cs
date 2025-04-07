using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
/// </summary>
public class Day4Authoring : MonoBehaviour
{
    public List<GameObject> lstObj;
    class Day4Baker : Baker<Day4Authoring>
    {
        public override void Bake(Day4Authoring authoring)
        {
            //获取实体
            var entity=GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day4Tag>(entity);
            //添加缓冲
            var buffer = AddBuffer<Day4BufferData>(entity);
            for (int i = 0; i < authoring.lstObj.Count; i++)
            {
                buffer.Add(new Day4BufferData { entity = GetEntity(authoring.lstObj[i], TransformUsageFlags.Dynamic) });
            }
        }
    }
}
