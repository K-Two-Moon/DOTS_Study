using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����ǩ���
/// </summary>
public struct  Day15_17Tag:IComponentData
{
}
/// <summary>
/// ʵ�������������
/// </summary>
public struct  Day15_17InsData:IComponentData
{
    public Entity tower;
    public Entity barrack;
    public Entity maincity;
    public Entity enemy;
    public Entity soldier;
    public Entity player;
}

/// <summary>
/// ��������
/// </summary>
public struct  Day15_17BuildingData:IComponentData
{
    public FixedString128Bytes bName;
    public float hp;
}

/// <summary>
/// Ѫ�����й����
/// </summary>
public class Day15_17Hp : IComponentData, IDisposable
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
/// ��ұ��
/// </summary>
public struct Day15_17PlayerTag:IComponentData
{
    
}

/// <summary>
/// ���˵��������
/// </summary>
public struct Day15_17EnemyData : IComponentData
{
    /// <summary>
    /// ���˵�����ֵ
    /// </summary>
    public float hp;

    /// <summary>
    /// ���˵��ƶ�Ŀ��λ��
    /// </summary>
    public float3 targetPos;

    /// <summary>
    /// ����ʱ����
    /// </summary>
    public float attackTimer;

    /// <summary>
    /// ������
    /// </summary>
    public float attackValue;
}

/// <summary>
/// ʿ�����
/// </summary>
public struct Day15_17SoldierData : IComponentData
{
    /// <summary>
    /// ʿ��������ֵ
    /// </summary>
    public float hp;

    /// <summary>
    /// ʿ�����ƶ�Ŀ��λ��
    /// </summary>
    public float3 targetPos;

    /// <summary>
    /// ����ʱ����
    /// </summary>
    public float attackTimer;

    /// <summary>
    /// ������
    /// </summary>
    public float attackValue;
}

/// <summary>
/// �ƶ����
/// </summary>
public struct  Day15_17MoveData:IComponentData
{
    public float speed;
    /// <summary>
    /// 0���ȴ��ƶ���1�������ƶ�
    /// </summary>
    public int selectFlag;
}