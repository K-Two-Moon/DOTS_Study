using System;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public InputField size;
    public Transform gridParent;
    void Start()
    {
        size.onEndEdit.AddListener(OnSetSize);
        InitGrid(10);
    }

    private void OnSetSize(string arg0)
    {
        if (!string.IsNullOrEmpty(arg0))
        {
            int size = int.Parse(arg0);
            InitGrid(size);
        }
    }

    private Collider[] GetSize(Vector3 pos, float size)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
        }
        Vector3 half = new Vector3(size, size, size);
        return Physics.OverlapBox(hit.point, half/2);
    }

    private void InitGrid(int size)
    {
        DestroyAllChild(gridParent);
        for (int i = 0; i < size; i++)
        {
            for (global::System.Int32 j = 0; j < size; j++)
            {
                Transform tran = Instantiate(Resources.Load<GameObject>("cell"), gridParent).transform;
                tran.position = new Vector3(i, 0, j);
                tran.name = "0";
            }
        }
    }

    private void DestroyAllChild(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
