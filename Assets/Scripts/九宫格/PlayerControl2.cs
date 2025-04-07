using UnityEngine;

public class PlayerControl2 : MonoBehaviour
{
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
        }
    }
}

