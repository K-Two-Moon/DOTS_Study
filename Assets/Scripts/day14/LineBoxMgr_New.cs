using UnityEngine;

/// <summary>
/// »®Ïß
/// </summary>
public class LineBoxMgr_New : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 v0, v1, v2,v3;
    public bool canDraw;
    private float zValue=3;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            v0 = Input.mousePosition;
            v0.z = zValue;
            canDraw = true;
            lr.positionCount = 4;
        }
        if (Input.GetMouseButton(0))
        {
            v2 = Input.mousePosition;
            v2.z = zValue;
        }
        if (Input.GetMouseButtonUp(0))
        {
            canDraw = false;
            lr.positionCount = 0;
        }
        if (canDraw)
        {
            v1 = new Vector3(v2.x,v0.y,zValue);
            v3 = new Vector3(v0.x,v2.y,zValue);
            lr.SetPosition(0, Camera.main.ScreenToWorldPoint(v0));
            lr.SetPosition(1, Camera.main.ScreenToWorldPoint(v1));
            lr.SetPosition(2, Camera.main.ScreenToWorldPoint(v2));
            lr.SetPosition(3, Camera.main.ScreenToWorldPoint(v3));
        }
    }
}
