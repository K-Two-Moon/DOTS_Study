using Unity.Entities;
using UnityEngine;

/// <summary>
/// 游戏入口
/// </summary>
public class Day5GameRoot : MonoBehaviour
{
    public static Day5GameRoot ins;
    /// <summary>
    /// 记录实体管理器
    /// </summary>
    public EntityManager em;
    /// <summary>
    /// 记录拖拽实体
    /// </summary>
    public Entity dragEntity;
    /// <summary>
    /// 记录实例化组件数据
    /// </summary>
    public Day5InsData data;
    /// <summary>
    /// 记录 拖拽的ui的名称
    /// </summary>
    public string dragUIName;
    private void Awake()
    {
        if (ins == null) { ins = this; }
    }
}
