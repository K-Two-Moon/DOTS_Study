using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ϸ���
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
            des.text = "��ͣ";
        }
        else
        {
            des.text = "����";
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Ϊʵ�����Ѫ���й����
    /// </summary>
    public void AddHpToEntity(EntityManager em, Entity entity, Vector3 pos)
    {
        //����Ѫ��
        var sli = Instantiate(Resources.Load<GameObject>("hp"), hpParrent).transform;
        sli.position = Camera.main.WorldToScreenPoint(pos+Vector3.up);
        em.AddComponentObject(entity, new Day6Hp { sli=sli.GetComponent<Slider>()});
    }
}
