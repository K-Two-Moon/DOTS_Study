using Unity.Entities;

/// <summary>
/// �����ǩ���
/// </summary>
public struct  DemoDataTag:IComponentData
{
    
}

/// <summary>
/// ����ʵ�����������
/// </summary>
public struct  DemoInsData:IComponentData
{
    /// <summary>
    /// ��Ҫʵ������cubeʵ��
    /// </summary>
    public Entity insCube;
}