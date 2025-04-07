using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ��ק����
/// </summary>
public class DragMgr : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
{
    /// <summary>
    /// �󶨿�ʼ��ק�¼�
    /// </summary>
    public event Action<PointerEventData> bindOnBeginDrag;
    /// <summary>
    /// ����ק���¼�
    /// </summary>
    public event Action<PointerEventData> bindOnDrag;
    /// <summary>
    /// ����ק�����¼�
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
    /// �������Ϊ��̬����ӻ��߻�ȡ�������ϵ����
    /// </summary>
    public static DragMgr Get(GameObject go)
    {
        DragMgr ins = null;
        if (go.TryGetComponent<DragMgr>(out ins))//���ҵ�go���ϵ�DragMgr������ҷ���
        {
        }
        else
        {
            //go����û�в��ҵ�DragMgr���������Ӳ�����
            ins=go.AddComponent<DragMgr>();
        }
        return ins;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bindOnPointerClick?.Invoke(eventData);
    }
}
