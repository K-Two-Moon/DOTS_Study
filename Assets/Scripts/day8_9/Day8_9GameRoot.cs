using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

/// <summary>
/// ��Ϸ���
/// </summary>
public class Day8_9GameRoot : MonoBehaviour
{
    public Text demondDes;
    public int dCount;
    public static Day8_9GameRoot ins;
    public Button btn_CreateCar, btn_Build;
    /// <summary>
    /// ��¼ʵ�������
    /// </summary>
    public EntityManager em;
    /// <summary>
    /// ��¼ʵ�����������
    /// </summary>
    public Day8_9InsData data;
    /// <summary>
    /// ��¼��ק��ʵ��
    /// </summary>
    public Entity dragEntity;

    /// <summary>
    /// ��¼��קui������
    /// </summary>
    public string dragUiName;

    /// <summary>
    /// �Ƿ�������
    /// </summary>
    public bool hasMainCity = true;

    /// <summary>
    /// �Ƿ���ʾ�t�����ĵ���ʱ
    /// </summary>
    public bool canShowViewTime = false;
    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        btn_CreateCar.onClick.AddListener(OnCreateCar);
        btn_Build.onClick.AddListener(OnBuildSoldier);
    }

    private Vector3 sPos;

    private void OnBuildSoldier()
    {
        btn_Build.gameObject.SetActive(false);
        //�ж��Ƿ���Դ���С��
        if (dCount >= 50)
        {
            var sEntity = em.Instantiate(data.soldier);
            //
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            var realPos = sPos + new Vector3(rndPos.x, 0, rndPos.y);
            em.SetComponentData(sEntity, new LocalTransform { Position = realPos, Scale = 1 });
            Day8_9GameRoot.ins.em.AddComponentData(sEntity, new Day8_9SoldierData { attackTime=1 });
            //���Ѫ��
            AddHpToEntity(em, sEntity, realPos);
            dCount -= 50;
            demondDes.text = $"��ʯ��{dCount}";
        }
    }

    public void AddHpToEntity(EntityManager em, Entity entity, Vector3 pos)
    {
        //����Ѫ��
        var sliTran = GameObject.Instantiate(Resources.Load<GameObject>("hp"), Day8_9GameRoot.ins.transform).transform;
        sliTran.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
        var sli = sliTran.GetComponent<Slider>();
        em.AddComponentObject(entity, new Day8_9HpData { sli = sli });
    }
    private void OnCreateCar()
    {

        btn_CreateCar.gameObject.SetActive(false);
        var carEntity = em.Instantiate(data.car);
        em.SetComponentData(carEntity, new LocalTransform { Position = new Unity.Mathematics.float3(-17, 0, -14), Scale = 100 });
        em.AddComponent<Day8_9CarTag>(carEntity);
    }

    /// <summary>
    /// ��ʾ����С��ui
    /// </summary>
    public void ShowSoldierUI(Vector3 pos)
    {
        sPos = pos;
        btn_Build.gameObject.SetActive(true);
        btn_Build.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.left * 5);
    }
    /// <summary>
    /// ��ʾ����С��ui
    /// </summary>
    public void ShowCarUI(Vector3 pos)
    {
        btn_CreateCar.gameObject.SetActive(true);
        btn_CreateCar.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.left * 5);
    }
}
