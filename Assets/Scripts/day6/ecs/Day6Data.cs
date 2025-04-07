using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ǩ
/// </summary>
public struct  Day6Tag:IComponentData
{
    
}

/// <summary>
/// ʵ�����������
/// </summary>
public struct  Day6InsData:IComponentData
{
    public Entity enemy;
    public Entity player;
    public Entity biu;
    public Entity bar;
    public Entity prop;
}

/// <summary>
/// ����Ѫ��
/// </summary>
public class Day6Hp : IComponentData, IDisposable
{
    public Slider sli;
    public void Dispose()
    {
        if (sli!=null)
        {
            GameObject.Destroy(sli.gameObject);
        }
    }
}

/// <summary>
/// �й����
/// </summary>
public class Day6AnimObj : IComponentData, IDisposable
{
    public Transform ins;
    public void Dispose()
    {
        if (ins != null) { GameObject.Destroy(ins.gameObject); }
    }
}

/// <summary>
/// ���˱�ǩ
/// </summary>
public struct  Day6EnemyTag:IComponentData
{
    
}

/// <summary>
/// ��ұ�ǩ
/// </summary>
public struct  Day6PlayerTag:IComponentData
{
    
}

/// <summary>
/// �ӵ��������
/// </summary>
public struct  Day6BiuData:IComponentData
{
    /// <summary>
    /// 0:bar,1:��ͨ
    /// </summary>
    public int type;
    public float livedTimer;
}

/// <summary>
/// �������
/// </summary>
public struct  Day6PropData:IComponentData
{
    public float maxScale;
    public float livedTimer;
}

/// <summary>
/// ʹ�õĵ��ұ�ǩ
/// </summary>
public struct  Day6UsePropData:IComponentData
{
    
}
