using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 标签 
/// </summary>
public struct  Day10Tag:IComponentData
{
    
}

/// <summary>
///实例化组件数据
/// </summary>
public struct  Day10InsData:IComponentData
{
    public Entity barrack;
    public Entity mine;
    public Entity soldier;
}

/// <summary>
/// 建筑数据
/// </summary>
public struct  Day10BuildingData:IComponentData
{
    public FixedString128Bytes des;
}

/// <summary>
/// 血条
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
/// 士兵的组件数据
/// </summary>
public struct  Day10SoldierData:IComponentData
{
    public float3 targetPos;
    public float hp;
}

/// <summary>
/// 移动数据
/// </summary>
public struct Day10MoveData:IComponentData
{
    public float3 targetPos;
}
