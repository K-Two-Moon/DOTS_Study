using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ǩ 
/// </summary>
public struct  Day10Tag:IComponentData
{
    
}

/// <summary>
///ʵ�����������
/// </summary>
public struct  Day10InsData:IComponentData
{
    public Entity barrack;
    public Entity mine;
    public Entity soldier;
}

/// <summary>
/// ��������
/// </summary>
public struct  Day10BuildingData:IComponentData
{
    public FixedString128Bytes des;
}

/// <summary>
/// Ѫ��
/// </summary>
public class Day10Hp : IComponentData, IDisposable
{
    public Slider sli;
    public void Dispose()
    {
        if (sli.gameObject!=null)
        {
            GameObject.Destroy(sli.gameObject);
        }
    }
}

/// <summary>
/// ʿ�����������
/// </summary>
public struct  Day10SoldierData:IComponentData
{
    public float3 targetPos;
    public float hp;
}

/// <summary>
/// �ƶ�����
/// </summary>
public struct Day10MoveData:IComponentData
{
    public float3 targetPos;
}
