using UnityEngine;
/// <summary>
/// 控制玩家
/// </summary>
public class PlayerControl : MonoBehaviour
{
    public MapManager mapManager;
    Camera cam;
    int mapScale;
    int pyl;
    int x = 0;
    int y = 0;
    float v;
    float h;
    float speed = 10;
    Vector3 gs = new Vector3(0, 4, -4);
    private void Awake()
    {
        cam = Camera.main;
        mapScale = mapManager.oneMapScale * 10;
        pyl = mapScale / 2;
        mapManager.CreatMap(x, y);
        cam.transform.position = transform.position + gs;
        cam.transform.LookAt(transform);
    }
    void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        if (v != 0 || h != 0)
        {
            transform.position += Vector3.forward * v * Time.deltaTime * speed;
            transform.position += Vector3.right * h * Time.deltaTime * speed;
            cam.transform.position = transform.position + gs;
            cam.transform.LookAt(transform);
            if (Mathf.Floor((transform.position.x + pyl) / mapScale) != x || Mathf.Floor((transform.position.z + pyl) / mapScale) != y)
            {
                x = (int)Mathf.Floor((transform.position.x + pyl) / mapScale);
                y = (int)Mathf.Floor((transform.position.z + pyl) / mapScale);
                mapManager.CreatMap(x, y);
            }
        }
    }
}

