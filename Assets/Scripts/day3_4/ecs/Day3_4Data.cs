using Unity.Entities;

/// <summary>
/// 标签
/// </summary>
public struct Day3_4Tag:IComponentData
{
    
}
/// <summary>
/// 实例化的组件数据
/// </summary>
public struct  Day3_4InsData:IComponentData
{
    public Entity player;
    public Entity enemy;
}

/// <summary>
/// 玩家
/// </summary>
public struct  Day3_4PlayerTag:IComponentData
{
    
}

/// <summary>
/// 敌人标签
/// </summary>
public struct  Day3_4EnemyTag:IComponentData
{
    
}