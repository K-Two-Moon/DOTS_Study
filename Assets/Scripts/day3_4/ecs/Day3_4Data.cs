using Unity.Entities;

/// <summary>
/// ��ǩ
/// </summary>
public struct Day3_4Tag:IComponentData
{
    
}
/// <summary>
/// ʵ�������������
/// </summary>
public struct  Day3_4InsData:IComponentData
{
    public Entity player;
    public Entity enemy;
}

/// <summary>
/// ���
/// </summary>
public struct  Day3_4PlayerTag:IComponentData
{
    
}

/// <summary>
/// ���˱�ǩ
/// </summary>
public struct  Day3_4EnemyTag:IComponentData
{
    
}