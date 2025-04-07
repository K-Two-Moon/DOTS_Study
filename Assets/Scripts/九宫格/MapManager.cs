using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图管理
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary>
    /// 地图块的比例
    /// </summary>
    [Range(1, 10)]
    public int oneMapScale;
    //存储所有的地图块
    Dictionary<int, Dictionary<int, GameObject>> allMaps = new Dictionary<int, Dictionary<int, GameObject>>();
    /// <summary>
    /// 地图池
    /// </summary>
    List<GameObject> mapPools = new List<GameObject>();
    /// <summary>
    /// 需要生成的地图块
    /// </summary>
    public GameObject map;
    
    /// <summary>
    /// 创建地图
    /// </summary>
    /// <param name="x">x的索引</param>
    /// <param name="y">y的索引</param>
    public void CreatMap(int x, int y)
    {
        //临时存储看到的地图
        List<GameObject> viewMap = new List<GameObject>();
        //遍历生成9宫格地图
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                viewMap.Add(CreatOMap(x + i, y + j));
            }
        }
        //遍历池子
        for (int i = 0; i < mapPools.Count; i++)
        {
            if (!viewMap.Contains(mapPools[i]))
            {
                mapPools[i].SetActive(false);
            }
        }
        mapPools.Clear();
        mapPools = viewMap;
        //显示
        for (int i = 0; i < mapPools.Count; i++)
        {
            mapPools[i].SetActive(true);
        }
    }

    /// <summary>
    /// 创建地图
    /// </summary>
    /// <param name="v1">纵向的索引</param>
    /// <param name="v2">横向的索引</param>
    /// <returns></returns>
    private GameObject CreatOMap(int v1, int v2)
    {
        if (allMaps.ContainsKey(v1))
        {
            if (!allMaps[v1].ContainsKey(v2))
            {
                GameObject go = Instantiate(map, transform);
                go.transform.position = new Vector3(v1 * 10 * oneMapScale, 0, v2 * 10 * oneMapScale);
                go.transform.GetChild(0).GetComponent<TextMesh>().text = "(" + v1 + "," + v2 + ")";
                go.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
                allMaps[v1].Add(v2, go);
            }
        }
        else
        {
            GameObject go = Instantiate(map, transform);
            go.transform.position = new Vector3(v1 * 10 * oneMapScale, 0, v2 * 10 * oneMapScale);
            go.transform.GetChild(0).GetComponent<TextMesh>().text = "(" + v1 + "," + v2 + ")";
            go.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
            Dictionary<int, GameObject> v2map = new Dictionary<int, GameObject>();
            v2map.Add(v2, go);
            allMaps.Add(v1, v2map);
        }
        return allMaps[v1][v2];
    }
}

