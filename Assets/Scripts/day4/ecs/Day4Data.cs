using Unity.Entities;

/// <summary>
/// �����ǩ���
/// </summary>
public struct  Day4Tag:IComponentData
{
    
}

/// <summary>
/// �������
/// </summary>
public struct  Day4BufferData:IBufferElementData
{
    public Entity entity;
}

/// <summary>
/// ������������
/// </summary>
public struct  Day4HpData:IComponentData,IEnableableComponent
{
    public float hp;
}