using Unity.Entities;

/// <summary>
/// 定义标签
/// </summary>
public struct  Day5Tag:IComponentData
{
    
}

/// <summary>
/// 实例化组件数据
/// </summary>
public struct  Day5InsData:IComponentData
{
    public Entity type1;
    public Entity type2;
}

/// <summary>
/// 建筑标签
/// </summary>
public struct  Day5BuildingTag:IComponentData
{
    
}