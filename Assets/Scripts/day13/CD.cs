using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 解析的数据实体
/// </summary>
public class ConfigData
{
    public float timer;
}

/// <summary>
/// cd
/// </summary>
public class CD : MonoBehaviour
{
    public Image img;
    public bool isFinished;
    private float cdTimer = 2;
    private Button btn;
    /// <summary>
    /// 是否可以进行cd
    /// </summary>
    public bool canCD;
    void Start()
    {
        btn=GetComponent<Button>();
        btn.onClick.AddListener(OnClicked);
    }

   /// <summary>
   /// 接受来自配置数据
   /// </summary>
   /// <param name="data"></param>
    public void InitData(ConfigData data)
    {
        //cdTimer = data.timer;
        cdTimer = 2;
    }
    private void OnClicked()
    {
        if (Day13GameRoot.ins.canCD)
        {
            Day13GameRoot.ins.canCD = false;
            canCD = true;
            img.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        ResetCDState();
    }
    void Update()
    {
        if (canCD)
        {
            cdTimer -= Time.deltaTime;

            img.fillAmount = cdTimer / 2;
            if (cdTimer <= 0)
            {
                isFinished=true;//是否可以拖拽
                ResetCDState();
            }
        }
    }

    /// <summary>
    /// 重置cd状态
    /// </summary>
    private void ResetCDState()
    {
        cdTimer = 2;
        img.gameObject.SetActive(false);
        img.fillAmount = 1;
        Day13GameRoot.ins.canCD = true;
    }
}
