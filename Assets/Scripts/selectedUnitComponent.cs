using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedUnitComponent : MonoBehaviour
{
    [SerializeField]
    private Vector3 pool_Position = new Vector3(0,-50,0);
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
       gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    void LateUpdate()
    {
        /*if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MoveToMousePoint();
        }*/
    }

    public void MoveToPoint(Vector3 destination)
    {
            navMeshAgent.destination =destination;
    }
    public void PlaceDestinationCircle()
    {

    }

    private void OnDestroy()
    {
       
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
