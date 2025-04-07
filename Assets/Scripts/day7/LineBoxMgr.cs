using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 框选
/// </summary>
public class LineBoxMgr : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 v0, v1, v2, v3;
    public float zValue=3;
    private bool canDrawBox = false;
     
    void Start()
    {
        lr= GetComponent<LineRenderer>();
    }
    void Update()
    {
        //按下
        if (Input.GetMouseButtonDown(0))
        {
            v0 = Input.mousePosition;
            v0.z= zValue;
            canDrawBox=true;
        }
        //按住
        if (Input.GetMouseButton(0))
        {
            v2= Input.mousePosition;
            v2.z= zValue;
            v1 = new Vector3(v2.x,v0.y,zValue);
            v3 = new Vector3(v0.x,v2.y,zValue);
        }
        if (Input.GetMouseButtonUp(0))
        {
            canDrawBox = false;
            lr.positionCount= 0;
            //
            var col = GetBoxCollider(GetHitPos(v0),GetHitPos(v2));
            for (global::System.Int32 i = 0; i < col.Length; i++)
            {
                //Debug.Log($"选中物体：{col[i].name}");
                if (col[i].name!="Plane")
                {
                    Destroy(col[i].gameObject);
                }
            }
        }
        if (canDrawBox)
        {
            
            //
            lr.positionCount = 4;
            lr.SetPosition(0, Camera.main.ScreenToWorldPoint(v0));
            lr.SetPosition(1, Camera.main.ScreenToWorldPoint(v1));
            lr.SetPosition(2, Camera.main.ScreenToWorldPoint(v2));
            lr.SetPosition(3, Camera.main.ScreenToWorldPoint(v3));
        }
    }

    //private Vector3 ts, th;
    private Collider[] GetBoxCollider(Vector3 start,Vector3 endpos)
    {
        Vector3 center = (start + endpos) / 2;
        Vector3 half = new Vector3(Mathf.Abs(endpos.x-start.x)/2,0.5f, Mathf.Abs(endpos.z - start.z) / 2);
        //ts = center;
        //th=half;
        
        return Physics.OverlapBox(center,half);
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(ts, th);
    //}
    private Vector3 GetHitPos(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
        }
        return hit.point;
    }
}
