using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 标签
/// </summary>
public struct  Day6Tag:IComponentData
{
    
}

/// <summary>
/// 实例化组件数据
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
/// 定义血条
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
/// 托管组件
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
/// 敌人标签
/// </summary>
public struct  Day6EnemyTag:IComponentData
{
    
}

/// <summary>
/// 玩家标签
/// </summary>
public struct  Day6PlayerTag:IComponentData
{
    
}

/// <summary>
/// 子弹组件数据
/// </summary>
public struct  Day6BiuData:IComponentData
{
    /// <summary>
    /// 0:bar,1:普通
    /// </summary>
    public int type;
    public float livedTimer;
}

/// <summary>
/// 道具组件
/// </summary>
public struct  Day6PropData:IComponentData
{
    public float maxScale;
    public float livedTimer;
}

/// <summary>
/// 使用的到家标签
/// </summary>
public struct  Day6UsePropData:IComponentData
{
    
}
