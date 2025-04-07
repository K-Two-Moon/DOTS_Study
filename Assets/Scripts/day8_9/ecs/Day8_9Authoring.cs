using Unity.Entities;
using UnityEngine;

/// <summary>
/// 创作数据
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
             //获取实体
             var entity=GetEntity(TransformUsageFlags.None);
            //添加标签
            AddComponent<Day8_9Tag>(entity);
            //添加组件
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
