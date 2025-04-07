using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// ÕÊº“π‹¿Ì
/// </summary>
public class PlayerMgr : MonoBehaviour
{
    private LineRenderer lr;
    private NavMeshAgent agent;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        if (agent.path.corners.Length>0)
        {
            lr.positionCount= agent.path.corners.Length;
            lr.SetPositions(agent.path.corners);
        }
    }
}
