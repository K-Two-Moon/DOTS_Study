using Unity.Entities.UniversalDelegates;
using UnityEngine;
 

public class Day18MapGenMgr : MonoBehaviour
{

    public Transform parent;
    /// <summary>
    /// ��ͼ����
    /// </summary>
    public int oneMapScale = 1;
    /// <summary>
    /// ��ʼ����ͼ
    /// </summary>
    public void InitData()
    {
        string mapData = Resources.Load<TextAsset>("mapconfig").text;  //ע�⣺��ÿ�еĽ�β��һ���ָ��Ƿ��š���������|��Ϊ�ָ
        string [] dataR = mapData.Split('|');
        for (int i = 0; i < dataR.Length; i++)
        {
            Debug.Log(dataR[i]);
            if (!string.IsNullOrEmpty(dataR[i]))
            {
                string[] dataC= dataR[i].Split(",");
                for (global::System.Int32 j = 0; j < dataC.Length; j++)
                {
                    switch (dataC[j])
                    {
                        case "1":
                            GetTran("1",i, j);
                            break;
                        case "2":
                            GetTran("2", i , j );
                            break;
                        case "3":
                            GetTran("3", i , j );
                            break;
                    }
                }
            }
        }
    }
    private void GetTran(string resName,int i,int j)
    {
        Transform tran = Instantiate(Resources.Load<GameObject>(resName), parent).transform;
        tran.position = new Vector3(i * 2 * oneMapScale, 0, j * 2 * oneMapScale);
    }
    void Start()
    {
        InitData();
    }
}
