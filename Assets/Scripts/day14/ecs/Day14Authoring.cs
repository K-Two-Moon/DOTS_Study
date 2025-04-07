using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Day14Authoring : MonoBehaviour
{
    public GameObject player;
    class Day14Baker : Baker<Day14Authoring>
    {
        public override void Bake(Day14Authoring authoring)
        {
            //��ȡʵ��
            var entity=GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day14Tag>(entity);
            //������
            AddComponent(entity, new Day14InsData {  player=GetEntity(authoring.player, TransformUsageFlags.Dynamic)});
        }
    }
}
