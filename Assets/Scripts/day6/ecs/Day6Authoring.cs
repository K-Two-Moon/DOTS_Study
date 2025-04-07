using Unity.Entities;
using UnityEngine;
/// <summary>
/// ��������
/// </summary>
public class Day6Authoring : MonoBehaviour
{
    public GameObject enemy, player, biu, bar, prop;
    class Day6Baker : Baker<Day6Authoring>
    {
        public override void Bake(Day6Authoring authoring)
        {

            //��ȡʵ��
            var entity = GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day6Tag>(entity);
            //������
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
