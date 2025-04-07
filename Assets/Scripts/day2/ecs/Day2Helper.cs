
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///����2�İ����ű�
/// </summary>
public class Day2Helper 
{
    /// <summary>
    /// ��¼���˵�����
    /// </summary>
    public static ushort eCount;
    private static Transform canvas = GameObject.Find("Canvas").transform;
    private static GameObject gameOverUi= canvas.Find("gameover").gameObject;

    /// <summary>
    /// Ϊʵ�����Ѫ�����
    /// </summary>
    /// <param name="em">ʵ�������</param>
    /// <param name="entity">��Ҫ���Ѫ����ʵ��</param>
    /// <param name="pos">ʵ���λ��</param>
    public static void AddHpToEntity(EntityManager em,Entity entity,Vector3 pos)
    {
        Transform tran = GameObject.Instantiate(Resources.Load<GameObject>("hp"), canvas).transform;
        tran.position = Camera.main.WorldToScreenPoint(pos+Vector3.up);
        var sli=tran.GetComponent<Slider>();
        em.AddComponentObject(entity, new Day2Hp{ sli=sli });
    }

    /// <summary>
    /// ��Ϸ�Ƿ����
    /// </summary>
    public static void GameOver(bool isShow)
    {
        gameOverUi.SetActive(isShow);
    }
}
