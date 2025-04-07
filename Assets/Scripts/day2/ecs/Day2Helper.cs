
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///案例2的帮助脚本
/// </summary>
public class Day2Helper 
{
    /// <summary>
    /// 记录敌人的数量
    /// </summary>
    public static ushort eCount;
    private static Transform canvas = GameObject.Find("Canvas").transform;
    private static GameObject gameOverUi= canvas.Find("gameover").gameObject;

    /// <summary>
    /// 为实体添加血条组件
    /// </summary>
    /// <param name="em">实体管理器</param>
    /// <param name="entity">需要添加血条的实体</param>
    /// <param name="pos">实体的位置</param>
    public static void AddHpToEntity(EntityManager em,Entity entity,Vector3 pos)
    {
        Transform tran = GameObject.Instantiate(Resources.Load<GameObject>("hp"), canvas).transform;
        tran.position = Camera.main.WorldToScreenPoint(pos+Vector3.up);
        var sli=tran.GetComponent<Slider>();
        em.AddComponentObject(entity, new Day2Hp{ sli=sli });
    }

    /// <summary>
    /// 游戏是否结束
    /// </summary>
    public static void GameOver(bool isShow)
    {
        gameOverUi.SetActive(isShow);
    }
}
