using System;
using System.Collections.Generic;
using UnityEngine;
//
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using Unity.Entities.UniversalDelegates;
using Unity.Rendering;
using Unity.Transforms;
/// <summary>
/// ����������
/// </summary>
public class BDataEntity
{
    public string bType;
    public string name;
    public float hp;
    public float size;
}

/// <summary>
/// ���˵�����
/// </summary>
public class EnemyDataEntity
{
    public float attackTimer;
    public float attackValue;
    public float size;
    public float hp;
    public float r, g, b, a;
}

/// <summary>
/// ��Ϸ���
/// </summary>
public class Day15_17GameRoot : MonoBehaviour
{
    public static Day15_17GameRoot ins;
    /// <summary>
    /// ���˵���������
    /// </summary>
    public List<EnemyDataEntity> eDataLst;
    /// <summary>
    /// ��������������
    /// </summary>
    public List<BDataEntity> bDataLst;

    /// <summary>
    /// ��ק��UI
    /// </summary>
    public List<GameObject> dragLst;

    /// <summary>
    /// ʵ�������
    /// </summary>
    public EntityManager em;

    /// <summary>
    /// ��ʱ��¼��ק��ʵ��
    /// </summary>
    public Entity dragEntity;

    /// <summary>
    /// ��¼ʵ�������������
    /// </summary>
    public Day15_17InsData insData;

    /// <summary>
    /// ��¼��ʱ�Ľ�������
    /// </summary>
    public BDataEntity tmpData;

    /// <summary>
    /// ��¼��ק��ui������
    /// </summary>
    public string uiName;
    private void Awake()
    {
        if (ins==null)
        {
            ins = this;
        }
        InitData();
    }
    public Text sDes, eDes;
    public int sCount,eCount;
    public Button btn_CreateSoldier;
    private void Start()
    {
        btn_CreateSoldier.onClick.AddListener(OnCreateSoldier);
    }

    private int index;
    private Vector3 barrackPos;
    private void OnCreateSoldier()
    {
        btn_CreateSoldier.gameObject.SetActive(false);
        //����С��ʵ��
        var entity = em.Instantiate(insData.soldier);
        //�������λ��
        var rndPos = UnityEngine.Random.insideUnitCircle * 5;
        var realPos = barrackPos + new Vector3(rndPos.x, 0, rndPos.y);
        index++;
        //�ı���ɫ
        if (index % 2 == 0)
        {
            var data = eDataLst[0];
            em.SetComponentData(entity, new LocalTransform { Position = realPos, Scale = data.size });
            em.SetComponentData(entity, new URPMaterialPropertyBaseColor { Value = new Unity.Mathematics.float4(data.r, data.g, data.b, data.a) });
        }
        else
        {
            var data = eDataLst[1];
            em.SetComponentData(entity, new LocalTransform { Position = realPos, Scale = data.size });
            em.SetComponentData(entity, new URPMaterialPropertyBaseColor { Value = new Unity.Mathematics.float4(data.r, data.g, data.b, data.a) });
        }
        //ΪС�����Ѫ��
        AddHpToEnity(em, entity, realPos);
        sCount++;
        UpdateSoldierCount();
    }

    /// <summary>
    /// ��ʾ���찴ť
    /// </summary>
    public void ShowCreateUI(Vector3 pos)
    {
        barrackPos = pos;//��¼λ��
        btn_CreateSoldier.gameObject.SetActive(true);
        btn_CreateSoldier.transform.position=Camera.main.WorldToScreenPoint(pos+Vector3.left*5);
    }
    /// <summary>
    /// ���µ��˵�����
    /// </summary>
    public void UpdateEnemyCount()
    {
        eDes.text = $"��ǰҰ�ֵ�������{eCount}";
    }
    /// <summary>
    /// ����ʿ��������
    /// </summary>
    public void UpdateSoldierCount()
    {
        sDes.text = $"С����ʣ��������{sCount}";
    }

    /// <summary>
    /// ��ʼ����������
    /// </summary>
    private void InitData()
    {
        var ejson = Resources.Load<TextAsset>("enmeyConfig").text;
        var bjson = Resources.Load<TextAsset>("bconfig").text;
        eDataLst=JsonConvert.DeserializeObject<List<EnemyDataEntity>>(ejson);
        bDataLst=JsonConvert.DeserializeObject<List<BDataEntity>>(bjson);
    }

    /// <summary>
    /// ���Ѫ��
    /// </summary>
    /// <param name="em"></param>
    /// <param name="entity"></param>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    public void AddHpToEnity(EntityManager em,Entity entity,Vector3 pos,string name="")
    {
        Transform tran = Instantiate(Resources.Load<GameObject>("newhp"), transform).transform;
        tran.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
        tran.Find("neck").GetComponent<Text>().text = name;
        var sli=tran.GetComponent<Slider>();    
        em.AddComponentObject(entity, new Day15_17Hp {  sli=sli});
    }
}
