using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图管理
/// </summary>
public class TerrainManager : MonoBehaviour
{
    /// <summary>
    /// 玩家对象
    /// </summary>
    public GameObject player;
    //玩家位置
    private Vector3 playerpos;
    /// <summary>
    /// 获取玩家所在位置的坐标
    /// </summary>
    public float PlayerWH;
    /// <summary>
    /// 获取地图所在位置的坐标
    /// </summary>
    public float TerrainWH;
    /// <summary>
    /// 预设
    /// </summary>
    public GameObject prefab;
    //显示的地形块
    private Dictionary<Vector2, GameObject> showDic = new
   Dictionary<Vector2, GameObject>();
    //对象池
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
            //需要显示的列表
            List<Vector2> showlist = new List<Vector2>();
            //创建玩家区域
            Rect playerRect = new Rect(player.transform.position.x,
           player.transform.position.z, PlayerWH, PlayerWH);
            //获取玩家所在
            int x = (int)(player.transform.position.x / TerrainWH);
            int z = (int)(player.transform.position.z / TerrainWH);
            showlist.Add(new Vector2(x, z));
            //右
            if (IsLap(playerRect, new Rect((x + 1) * TerrainWH, z *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x + 1, z));
            }
            //左
            if (IsLap(playerRect, new Rect((x - 1) * TerrainWH, z *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x - 1, z));
            }
            //前
            if (IsLap(playerRect, new Rect(x * TerrainWH, (z + 1) *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x, z + 1));
            }
            //后
            if (IsLap(playerRect, new Rect(x * TerrainWH, (z - 1) *
           TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x, z - 1));
            }
            //右前
            if (IsLap(playerRect, new Rect((x + 1) * TerrainWH, (z +
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x + 1, z + 1));
            }
            //左前
            if (IsLap(playerRect, new Rect((x - 1) * TerrainWH, (z +
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x - 1, z + 1));
            }
            //右后
            if (IsLap(playerRect, new Rect((x + 1) * TerrainWH, (z -
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x + 1, z - 1));
            }
            //左后
            if (IsLap(playerRect, new Rect((x - 1) * TerrainWH, (z -
           1) * TerrainWH, TerrainWH, TerrainWH))) ;
            {
                showlist.Add(new Vector2(x - 1, z - 1));
            }
            //需要删掉的集合
            List<Vector2> deslist = new List<Vector2>();
            //从正在显示的里面找到不需要显示的
            foreach (var item in showDic.Keys)
            {
                if (!showlist.Contains(item))
                {
                    //隐藏并存入对象池
                    showDic[item].SetActive(false);
                    pool.Enqueue(showDic[item]);
                    deslist.Add(item);
                }
            }
            //从字典中删除
            foreach (var item in deslist)
            {
                showDic.Remove(item);
            }
            //找到需要显示但没显示的
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
                    //修改一下显示位置
                    terrain.transform.Find("GameObject").GetComponent<TextMesh>().text = $"({item.x},{item.y})";
                    showDic.Add(item, terrain);
                }
            }
        }
        playerpos = player.transform.position;
    }
    /// <summary>
    /// 计算是否与玩家检测区域重合的方法
    /// </summary>
    /// <param name="a">矩形边界 a</param>
    /// <param name="b">矩形边界 b</param>
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