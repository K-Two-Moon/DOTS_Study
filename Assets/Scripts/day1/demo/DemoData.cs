using Unity.Entities;

/// <summary>
/// 定义标签组件
/// </summary>
public struct  DemoDataTag:IComponentData
{
    
}

/// <summary>
/// 定义实例化组件数据
/// </summary>
public struct  DemoInsData:IComponentData
{
    /// <summary>
    /// 需要实例化的cube实体
    /// </summary>
    public Entity insCube;
}