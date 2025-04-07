using Unity.Entities;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class Day8_9Authoring : MonoBehaviour
{
    public GameObject mine;
    public GameObject barrack;
    public GameObject maincity;
    public GameObject tower;
    public GameObject tec;
    public GameObject soldier;
    public GameObject enemy;
    public GameObject car;
    class Day8_9Baker : Baker<Day8_9Authoring>
    {
        public override void Bake(Day8_9Authoring authoring)
        {
             //��ȡʵ��
             var entity=GetEntity(TransformUsageFlags.None);
            //��ӱ�ǩ
            AddComponent<Day8_9Tag>(entity);
            //������
            AddComponent(entity, new Day8_9InsData { 
             mine=GetEntity(authoring.mine, TransformUsageFlags.Dynamic),
             barrack=GetEntity(authoring.barrack, TransformUsageFlags.Dynamic),
             maincity=GetEntity(authoring.maincity, TransformUsageFlags.Dynamic),
             tower=GetEntity(authoring.tower, TransformUsageFlags.Dynamic),
             tec=GetEntity(authoring.tec, TransformUsageFlags.Dynamic),
             soldier=GetEntity(authoring.soldier, TransformUsageFlags.Dynamic),
             enemy=GetEntity(authoring.enemy, TransformUsageFlags.Dynamic),
             car=GetEntity(authoring.car, TransformUsageFlags.Dynamic),
            });
        }
    }
}
