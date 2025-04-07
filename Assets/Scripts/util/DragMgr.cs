using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 拖拽管理
/// </summary>
public class DragMgr : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
{
    /// <summary>
    /// 绑定开始拖拽事件
    /// </summary>
    public event Action<PointerEventData> bindOnBeginDrag;
    /// <summary>
    /// 绑定拖拽中事件
    /// </summary>
    public event Action<PointerEventData> bindOnDrag;
    /// <summary>
    /// 绑定拖拽结束事件
    /// </summary>
    public event Action<PointerEventData> bindOnEndDrag;


    public event Action<PointerEventData> bindOnPointerClick;

    public void OnBeginDrag(PointerEventData eventData)
    {
        bindOnBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        bindOnDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bindOnEndDrag?.Invoke(eventData);
    }

    /// <summary>
    /// 可以理解为动态的添加或者获取对象身上的组件
    /// </summary>
    public static DragMgr Get(GameObject go)
    {
        DragMgr ins = null;
        if (go.TryGetComponent<DragMgr>(out ins))//查找到go身上的DragMgr组件并且返回
        {
        }
        else
        {
            //go身上没有查找到DragMgr组件，则添加并返回
            ins=go.AddComponent<DragMgr>();
        }
        return ins;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bindOnPointerClick?.Invoke(eventData);
    }
}
