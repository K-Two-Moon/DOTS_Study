using Unity.Entities;

public struct Day14Tag:IComponentData
{
    
}

public struct  Day14InsData:IComponentData
{
    public Entity player;
}

/// <summary>
/// ��ҵ��������
/// </summary>
public struct  Day14PlayerData:IComponentData
{
    public float hp;
}