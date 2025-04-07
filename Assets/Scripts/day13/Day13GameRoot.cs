using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 单利入口
/// </summary>
public class Day13GameRoot : MonoBehaviour
{
    public List<GameObject> subUIObj;
    public Toggle[] toggles;
    public static Day13GameRoot ins;
    public bool canCD=true;
    private void Awake()
    {
        if (ins==null)
        {
            ins = this;
        }
    }
    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            Toggle toggle = toggles[i];
            toggle.onValueChanged.AddListener((isShow) => { ChangeToggle(isShow,toggle); });
        }
        subUIObj[0].gameObject.SetActive(true);
    }

    private void ChangeToggle(bool isShow, Toggle toggle)
    {
        for (int i = 0; i < subUIObj.Count; i++)
        {
            if (subUIObj[i].name == toggle.name)
            {
                subUIObj[i].gameObject.SetActive(true);
            }
            else
            {
                subUIObj[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 改变开关的可交互性
    /// </summary>
    public void ChangeUIState(Toggle t,bool state)
    {
        t.interactable= state;
    }
}
