using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ҡ��
/// </summary>
public class ETC : MonoBehaviour, IEndDragHandler, IDragHandler
{
    //��¼�����
    Vector3 outstart;
    float dis;//����
    float R = 100;//�뾶
    Vector3 dir;//����
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
