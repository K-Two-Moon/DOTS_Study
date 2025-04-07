using Unity.Entities;

/// <summary>
/// 定义标签组件
/// </summary>
public struct  Day4Tag:IComponentData
{
    
}

/// <summary>
/// 缓冲组件
/// </summary>
public struct  Day4BufferData:IBufferElementData
{
    public Entity entity;
}

/// <summary>
/// 定义可启用组件
/// </summary>
public struct  Day4HpData:IComponentData,IEnableableComponent
{
    public float hp;
}