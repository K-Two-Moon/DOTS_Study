using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 游戏入口
/// </summary>
public class Day8_9GameRoot : MonoBehaviour
{
    public Text demondDes;
    public int dCount;
    public static Day8_9GameRoot ins;
    public Button btn_CreateCar, btn_Build;
    /// <summary>
    /// 记录实体管理器
    /// </summary>
    public EntityManager em;
    /// <summary>
    /// 记录实例化组件数据
    /// </summary>
    public Day8_9InsData data;
    /// <summary>
    /// 记录拖拽的实体
    /// </summary>
    public Entity dragEntity;

    /// <summary>
    /// 记录拖拽ui的名称
    /// </summary>
    public string dragUiName;

    /// <summary>
    /// 是否有主城
    /// </summary>
    public bool hasMainCity = true;

    /// <summary>
    /// 是否显示瞭望塔的倒计时
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
        //判断是否可以创建小兵
        if (dCount >= 50)
        {
            var sEntity = em.Instantiate(data.soldier);
            //
            var rndPos = UnityEngine.Random.insideUnitCircle * 5;
            var realPos = sPos + new Vector3(rndPos.x, 0, rndPos.y);
            em.SetComponentData(sEntity, new LocalTransform { Position = realPos, Scale = 1 });
            Day8_9GameRoot.ins.em.AddComponentData(sEntity, new Day8_9SoldierData { attackTime=1 });
            //添加血条
            AddHpToEntity(em, sEntity, realPos);
            dCount -= 50;
            demondDes.text = $"钻石：{dCount}";
        }
    }

    public void AddHpToEntity(EntityManager em, Entity entity, Vector3 pos)
    {
        //生成血条
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
    /// 显示见着小兵ui
    /// </summary>
    public void ShowSoldierUI(Vector3 pos)
    {
        sPos = pos;
        btn_Build.gameObject.SetActive(true);
        btn_Build.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.left * 5);
    }
    /// <summary>
    /// 显示创建小车ui
    /// </summary>
    public void ShowCarUI(Vector3 pos)
    {
        btn_CreateCar.gameObject.SetActive(true);
        btn_CreateCar.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.left * 5);
    }
}
