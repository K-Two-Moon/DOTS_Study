using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏入口
/// </summary>
public class Day6GameRoot : MonoBehaviour
{
    public static Day6GameRoot ins;
    public Transform[] spawnLst;
    public Transform hpParrent;
    public Toggle t;
    public Text des;
    public Text killCountDes,scoreDes;
    public int killCount, socreCount;
    private void Awake()
    {
        if (ins == null) { ins = this; }
        t.onValueChanged.AddListener(OnChange);
    }

    private void OnChange(bool arg0)
    {
        if (arg0)
        {
            Time.timeScale = 1;
            des.text = "暂停";
        }
        else
        {
            des.text = "播放";
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// 为实体添加血条托管组件
    /// </summary>
    public void AddHpToEntity(EntityManager em, Entity entity, Vector3 pos)
    {
        //生成血条
        var sli = Instantiate(Resources.Load<GameObject>("hp"), hpParrent).transform;
        sli.position = Camera.main.WorldToScreenPoint(pos+Vector3.up);
        em.AddComponentObject(entity, new Day6Hp { sli=sli.GetComponent<Slider>()});
    }
}
