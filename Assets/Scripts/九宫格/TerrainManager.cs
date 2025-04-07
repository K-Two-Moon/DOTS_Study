using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͼ����
/// </summary>
public class TerrainManager : MonoBehaviour
{
    /// <summary>
    /// ��Ҷ���
    /// </summary>
    public GameObject player;
    //���λ��
    private Vector3 playerpos;
    /// <summary>
    /// ��ȡ�������λ�õ�����
    /// </summary>
    public float PlayerWH;
    /// <summary>
    /// ��ȡ��ͼ����λ�õ�����
    /// </summary>
    public float TerrainWH;
    /// <summary>
    /// Ԥ��
    /// </summary>
    public GameObject prefab;
    //��ʾ�ĵ��ο�
    private Dictionary<Vector2, GameObject> showDic = new
   Dictionary<Vector2, GameObject>();
    //�����
    private Queue<GameObject> pool = new Queue<GameObject>();
    void Start()
    {
        playerpos = player.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerpos != player.transform.position)
        {
            //��Ҫ��ʾ���б�
            List<Vector2> showlist = new List<Vector2>();
            //�����������
            Rect playerRect = new Rect(player.transform.position.x,
           player.transform.position.z, PlayerWH, PlayerWH);
            //��ȡ�������
            int x = (int)(player.transform.position.x / TerrainWH);
            int z = (int)(player.transform.position.z / TerrainWH);
            showlist.Add(new Vector2(x, z));
            //��
            if (IsLap(playerRect, new Rect((x + 1) * TerrainWH, z *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x + 1, z));
            }
            //��
            if (IsLap(playerRect, new Rect((x - 1) * TerrainWH, z *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x - 1, z));
            }
            //ǰ
            if (IsLap(playerRect, new Rect(x * TerrainWH, (z + 1) *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x, z + 1));
            }
            //��
            if (IsLap(playerRect, new Rect(x * TerrainWH, (z - 1) *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x, z - 1));
            }
            //��ǰ
            if (IsLap(playerRect, new Rect((x + 1) * TerrainWH, (z +
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x + 1, z + 1));
            }
            //��ǰ
            if (IsLap(playerRect, new Rect((x - 1) * TerrainWH, (z +
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x - 1, z + 1));
            }
            //�Һ�
            if (IsLap(playerRect, new Rect((x + 1) * TerrainWH, (z -
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x + 1, z - 1));
            }
            //���
            if (IsLap(playerRect, new Rect((x - 1) * TerrainWH, (z -
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x - 1, z - 1));
            }
            //��Ҫɾ���ļ���
            List<Vector2> deslist = new List<Vector2>();
            //��������ʾ�������ҵ�����Ҫ��ʾ��
            foreach (var item in showDic.Keys)
            {
                if (!showlist.Contains(item))
                {
                    //���ز���������
                    showDic[item].SetActive(false);
                    pool.Enqueue(showDic[item]);
                    deslist.Add(item);
                }
            }
            //���ֵ���ɾ��
            foreach (var item in deslist)
            {
                showDic.Remove(item);
            }
            //�ҵ���Ҫ��ʾ��û��ʾ��
            foreach (var item in showlist)
            {
                if (!showDic.ContainsKey(item))
                {
                    GameObject terrain;
                    if (pool.Count > 0)
                    {
                        terrain = pool.Dequeue();
                        terrain.SetActive(true);
                    }
                    else
                    {
                        terrain = Instantiate(prefab);
                    }
                    terrain.transform.position = new Vector3(item.x *
                   TerrainWH, 0, item.y * TerrainWH);
                    //�޸�һ����ʾλ��
                    terrain.transform.Find("GameObject").GetComponent<TextMesh>().text = $"({item.x},{item.y})";
                    showDic.Add(item, terrain);
                }
            }
        }
        playerpos = player.transform.position;
    }
    /// <summary>
    /// �����Ƿ�����Ҽ�������غϵķ���
    /// </summary>
    /// <param name="a">���α߽� a</param>
    /// <param name="b">���α߽� b</param>
    /// <returns></returns>
    public bool IsLap(Rect a, Rect b)
    {
        float aMinX = a.x - a.width / 2;
        float aMaxX = a.x + a.width / 2;
        float aMinZ = a.y - a.height / 2;
        float aMaxZ = a.y + a.height / 2;
        float bMinX = b.x - b.width / 2;
        float bMaxX = b.x + b.width / 2;
        float bMinZ = b.y - b.height / 2;
        float bMaxZ = b.y + b.height / 2;
        if (aMinX < bMaxX &&
        bMinX < aMaxX &&
        aMinZ < bMaxZ &&
        bMinZ < aMaxZ)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}