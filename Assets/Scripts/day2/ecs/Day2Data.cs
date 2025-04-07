using System;
using System.Numerics;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 定义标签
/// </summary>
public struct  Day2Tag:IComponentData { }

/// <summary>
/// 实例化组件数据
/// </summary>
public struct  Day2InsData:IComponentData
{
    /// <summary>
    /// 需要生成的玩家
    /// </summary>
    public Entity player;
    /// <summary>
    /// 需要生成的敌人
    /// </summary>
    public Entity enemy;

    /// <summary>
    /// 需要生成的子弹
    /// </summary>
    public Entity biu;

    /// <summary>
    /// 掉落
    /// </summary>
    public Entity drop;
}

/// <summary>
/// 玩家组件数据
/// </summary>
public struct Day2PlayerData:IComponentData
{
    /// <summary>
    /// 生命值
    /// </summary>
    public float hp;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;
}

/// <summary>
/// 敌人组件数据
/// </summary>
public struct  Day2EnemyData:IComponentData
{
    /// <summary>
    /// 敌人生命值
    /// </summary>
    public float hp;

    /// <summary>
    /// 敌人的目标位置
    /// </summary>
    public float3 targetPos;

    /// <summary>
    /// 攻击时间间隔
    /// </summary>
    public float attackTimer;

    /// <summary>
    /// 攻击玩家时掉血的数值
    /// </summary>
    public float attacValue;
}

/// <summary>
/// 子弹组件数据
/// </summary>
public struct  Day2BiuData:IComponentData
{
    /// <summary>
    /// 子弹的存活时间
    /// </summary>
    public float livedTimer;

    /// <summary>
    /// 子弹的移动速度
    /// </summary>
    public float moveSpeed;
}

/// <summary>
/// 物品掉落标签
/// </summary>
public struct  Day2DropTag:IComponentData
{
}

/// <summary>
/// 血条的托管组件
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
