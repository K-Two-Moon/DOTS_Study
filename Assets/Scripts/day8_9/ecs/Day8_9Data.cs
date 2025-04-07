using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����ǩ
/// </summary>
public struct  Day8_9Tag:IComponentData
{
    
}

/// <summary>
/// ʵ�����������
/// </summary>
public struct  Day8_9InsData:IComponentData
{
    public Entity mine;
    public Entity barrack;
    public Entity maincity;
    public Entity tower;
    public Entity tec;
    public Entity soldier;
    public Entity enemy;
    public Entity car;
}

/// <summary>
/// ������Ѫ��������
/// </summary>
public class Day8_9HpData : IComponentData, IDisposable
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
/// ����ö��
/// </summary>
public enum Day8_9BuilingType:ushort
{
    /// <summary>
    /// ��
    /// </summary>
    None,
    /// <summary>
    /// ����
    /// </summary>
    MainCity,

    /// <summary>
    /// ������
    /// </summary>
    Factory,
    /// <summary>
    /// �t����
    /// </summary>
    Tower,

    /// <summary>
    /// �Ƽ�Ӫ
    /// </summary>
    Tec
}

/// <summary>
/// �����������
/// </summary>
public struct  Day8_9BuildingData:IComponentData
{
    public Day8_9BuilingType bType;
    public FixedString128Bytes name;
}

/// <summary>
/// С����ǩ
/// </summary>
public struct  Day8_9CarTag:IComponentData
{
    
}

/// <summary>
/// С�����
/// </summary>
public struct Day8_9SoldierData:IComponentData
{
    /// <summary>
    /// ����ʱ����
    /// </summary>
    public float attackTime;
}

/// <summary>
/// ʿ���������
/// </summary>
public struct  Day8_9SoldierMoveData:IComponentData
{
    /// <summary>
    /// ʿ����Ŀ��λ��
    /// </summary>
    public float3 targetPos;
}

/// <summary>
/// �������
/// </summary>
public struct  Day8_9EnemyData:IComponentData
{
    /// <summary>
    /// ����ʱ����
    /// </summary>
    public float attackTime;
    /// <summary>
    /// ����Ѫ��
    /// </summary>
    public float hp;

    /// <summary>
    /// �����ƶ�����Ŀ��λ��
    /// </summary>
    public float3 targetPos;
}