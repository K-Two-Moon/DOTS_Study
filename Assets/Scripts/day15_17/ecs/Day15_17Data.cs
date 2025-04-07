using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 定义标签组件
/// </summary>
public struct  Day15_17Tag:IComponentData
{
}
/// <summary>
/// 实例化的组件数据
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
/// 建筑数据
/// </summary>
public struct  Day15_17BuildingData:IComponentData
{
    public FixedString128Bytes bName;
    public float hp;
}

/// <summary>
/// 血条的托管组件
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
/// 玩家标记
/// </summary>
public struct Day15_17PlayerTag:IComponentData
{
    
}

/// <summary>
/// 敌人的组件数据
/// </summary>
public struct Day15_17EnemyData : IComponentData
{
    /// <summary>
    /// 敌人的生命值
    /// </summary>
    public float hp;

    /// <summary>
    /// 敌人的移动目标位置
    /// </summary>
    public float3 targetPos;

    /// <summary>
    /// 攻击时间间隔
    /// </summary>
    public float attackTimer;

    /// <summary>
    /// 攻击力
    /// </summary>
    public float attackValue;
}

/// <summary>
/// 士兵组件
/// </summary>
public struct Day15_17SoldierData : IComponentData
{
    /// <summary>
    /// 士兵的生命值
    /// </summary>
    public float hp;

    /// <summary>
    /// 士兵的移动目标位置
    /// </summary>
    public float3 targetPos;

    /// <summary>
    /// 攻击时间间隔
    /// </summary>
    public float attackTimer;

    /// <summary>
    /// 攻击力
    /// </summary>
    public float attackValue;
}

/// <summary>
/// 移动组件
/// </summary>
public struct  Day15_17MoveData:IComponentData
{
    public float speed;
    /// <summary>
    /// 0：等待移动，1：可以移动
    /// </summary>
    public int selectFlag;
}