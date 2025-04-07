using Unity.Entities;
using UnityEngine;

public class Day15_17Authoring : MonoBehaviour
{
    public GameObject tower;
    public GameObject barrack;
    public GameObject maincity;
    public GameObject enemy;
    public GameObject soldier;
    public GameObject player;
    class Day15_17Baker : Baker<Day15_17Authoring>
    {
        public override void Bake(Day15_17Authoring authoring)
        {
            //��ȡʵ��
            var entity = GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day15_17Tag>(entity);
            //������
            AddComponent(entity, new Day15_17InsData
            {

                tower = GetEntity(authoring.tower, TransformUsageFlags.Dynamic),
                barrack = GetEntity(authoring.barrack, TransformUsageFlags.Dynamic),
                maincity = GetEntity(authoring.maincity, TransformUsageFlags.Dynamic),
                enemy = GetEntity(authoring.enemy, TransformUsageFlags.Dynamic),
                soldier = GetEntity(authoring.soldier, TransformUsageFlags.Dynamic),
                player = GetEntity(authoring.player, TransformUsageFlags.Dynamic),
            });
        }
    }
}
