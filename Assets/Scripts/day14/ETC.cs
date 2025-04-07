using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 摇杆
/// </summary>
public class ETC : MonoBehaviour, IEndDragHandler, IDragHandler
{
    //记录的起点
    Vector3 outstart;
    float dis;//距离
    float R = 100;//半径
    Vector3 dir;//方向
    RectTransform them;
    public static ETC ins;
    private void Awake()
    {
        ins = this;
    }
    public void OnDrag(PointerEventData eventData)
    {
        dis = Vector3.Distance(Input.mousePosition, outstart);
        if(dis<R)
        {
            transform.position = Input.mousePosition;
        }
        else
        {
            dir = Input.mousePosition - outstart;
            transform.position = dir.normalized * R + outstart;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = outstart;
    }
    void Start()
    {
        outstart = transform.position;
        them = GetComponent<RectTransform>();
    }
    public float GetAxis(string name)
    {
        if(name == "H")
        {
            return them.anchoredPosition.x / R;
        }
        else if(name == "V")
        {
            return them.anchoredPosition.y / R;
        }
        return 0;
    }    
}
