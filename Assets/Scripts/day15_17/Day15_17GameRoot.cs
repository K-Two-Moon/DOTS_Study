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
/// 建筑的数据
/// </summary>
public class BDataEntity
{
    public string bType;
    public string name;
    public float hp;
    public float size;
}

/// <summary>
/// 敌人的数据
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
/// 游戏入口
/// </summary>
public class Day15_17GameRoot : MonoBehaviour
{
    public static Day15_17GameRoot ins;
    /// <summary>
    /// 敌人的配置数据
    /// </summary>
    public List<EnemyDataEntity> eDataLst;
    /// <summary>
    /// 建筑的配置数据
    /// </summary>
    public List<BDataEntity> bDataLst;

    /// <summary>
    /// 拖拽的UI
    /// </summary>
    public List<GameObject> dragLst;

    /// <summary>
    /// 实体管理器
    /// </summary>
    public EntityManager em;

    /// <summary>
    /// 临时记录拖拽的实体
    /// </summary>
    public Entity dragEntity;

    /// <summary>
    /// 记录实例化的组件数据
    /// </summary>
    public Day15_17InsData insData;

    /// <summary>
    /// 记录临时的建筑数据
    /// </summary>
    public BDataEntity tmpData;

    /// <summary>
    /// 记录拖拽的ui的名称
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
        //生成小兵实体
        var entity = em.Instantiate(insData.soldier);
        //设置随机位置
        var rndPos = UnityEngine.Random.insideUnitCircle * 5;
        var realPos = barrackPos + new Vector3(rndPos.x, 0, rndPos.y);
        index++;
        //改变颜色
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
        //为小兵添加血条
        AddHpToEnity(em, entity, realPos);
        sCount++;
        UpdateSoldierCount();
    }

    /// <summary>
    /// 显示建造按钮
    /// </summary>
    public void ShowCreateUI(Vector3 pos)
    {
        barrackPos = pos;//记录位置
        btn_CreateSoldier.gameObject.SetActive(true);
        btn_CreateSoldier.transform.position=Camera.main.WorldToScreenPoint(pos+Vector3.left*5);
    }
    /// <summary>
    /// 更新敌人的数量
    /// </summary>
    public void UpdateEnemyCount()
    {
        eDes.text = $"当前野怪的数量：{eCount}";
    }
    /// <summary>
    /// 更新士兵的数量
    /// </summary>
    public void UpdateSoldierCount()
    {
        sDes.text = $"小兵的剩余数量：{sCount}";
    }

    /// <summary>
    /// 初始化配置数据
    /// </summary>
    private void InitData()
    {
        var ejson = Resources.Load<TextAsset>("enmeyConfig").text;
        var bjson = Resources.Load<TextAsset>("bconfig").text;
        eDataLst=JsonConvert.DeserializeObject<List<EnemyDataEntity>>(ejson);
        bDataLst=JsonConvert.DeserializeObject<List<BDataEntity>>(bjson);
    }

    /// <summary>
    /// 添加血条
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
