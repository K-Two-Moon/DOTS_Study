using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Day4Authoring : MonoBehaviour
{
    public List<GameObject> lstObj;
    class Day4Baker : Baker<Day4Authoring>
    {
        public override void Bake(Day4Authoring authoring)
        {
            //��ȡʵ��
            var entity=GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day4Tag>(entity);
            //��ӻ���
            var buffer = AddBuffer<Day4BufferData>(entity);
            for (int i = 0; i < authoring.lstObj.Count; i++)
            {
                buffer.Add(new Day4BufferData { entity = GetEntity(authoring.lstObj[i], TransformUsageFlags.Dynamic) });
            }
        }
    }
}
