using System;
using System.Numerics;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����ǩ
/// </summary>
public struct  Day2Tag:IComponentData { }

/// <summary>
/// ʵ�����������
/// </summary>
public struct  Day2InsData:IComponentData
{
    /// <summary>
    /// ��Ҫ���ɵ����
    /// </summary>
    public Entity player;
    /// <summary>
    /// ��Ҫ���ɵĵ���
    /// </summary>
    public Entity enemy;

    /// <summary>
    /// ��Ҫ���ɵ��ӵ�
    /// </summary>
    public Entity biu;

    /// <summary>
    /// ����
    /// </summary>
    public Entity drop;
}

/// <summary>
/// ����������
/// </summary>
public struct Day2PlayerData:IComponentData
{
    /// <summary>
    /// ����ֵ
    /// </summary>
    public float hp;
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public float moveSpeed;
}

/// <summary>
/// �����������
/// </summary>
public struct  Day2EnemyData:IComponentData
{
    /// <summary>
    /// ��������ֵ
    /// </summary>
    public float hp;

    /// <summary>
    /// ���˵�Ŀ��λ��
    /// </summary>
    public float3 targetPos;

    /// <summary>
    /// ����ʱ����
    /// </summary>
    public float attackTimer;

    /// <summary>
    /// �������ʱ��Ѫ����ֵ
    /// </summary>
    public float attacValue;
}

/// <summary>
/// �ӵ��������
/// </summary>
public struct  Day2BiuData:IComponentData
{
    /// <summary>
    /// �ӵ��Ĵ��ʱ��
    /// </summary>
    public float livedTimer;

    /// <summary>
    /// �ӵ����ƶ��ٶ�
    /// </summary>
    public float moveSpeed;
}

/// <summary>
/// ��Ʒ�����ǩ
/// </summary>
public struct  Day2DropTag:IComponentData
{
}

/// <summary>
/// Ѫ�����й����
/// </summary>
public class Day2Hp : IComponentData, IDisposable
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
