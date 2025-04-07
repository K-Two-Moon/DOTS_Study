using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 定义标签
/// </summary>
public struct  Day8_9Tag:IComponentData
{
    
}

/// <summary>
/// 实例化组件数据
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
/// 建筑的血条及描述
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
/// 建筑枚举
/// </summary>
public enum Day8_9BuilingType:ushort
{
    /// <summary>
    /// 无
    /// </summary>
    None,
    /// <summary>
    /// 主城
    /// </summary>
    MainCity,

    /// <summary>
    /// 兵工厂
    /// </summary>
    Factory,
    /// <summary>
    /// 瞭望塔
    /// </summary>
    Tower,

    /// <summary>
    /// 科技营
    /// </summary>
    Tec
}

/// <summary>
/// 建筑组件数据
/// </summary>
public struct  Day8_9BuildingData:IComponentData
{
    public Day8_9BuilingType bType;
    public FixedString128Bytes name;
}

/// <summary>
/// 小车标签
/// </summary>
public struct  Day8_9CarTag:IComponentData
{
    
}

/// <summary>
/// 小兵组件
/// </summary>
public struct Day8_9SoldierData:IComponentData
{
    /// <summary>
    /// 攻击时间间隔
    /// </summary>
    public float attackTime;
}

/// <summary>
/// 士兵组件数据
/// </summary>
public struct  Day8_9SoldierMoveData:IComponentData
{
    /// <summary>
    /// 士兵的目标位置
    /// </summary>
    public float3 targetPos;
}

/// <summary>
/// 敌人组件
/// </summary>
public struct  Day8_9EnemyData:IComponentData
{
    /// <summary>
    /// 攻击时间间隔
    /// </summary>
    public float attackTime;
    /// <summary>
    /// 敌人血量
    /// </summary>
    public float hp;

    /// <summary>
    /// 敌人移动到的目标位置
    /// </summary>
    public float3 targetPos;
}