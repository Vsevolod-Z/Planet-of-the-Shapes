using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedUnitComponent : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        mainCamera = Camera.main;
        
       gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MoveToMousePoint();
        }
    }

    private void MoveToMousePoint()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            navMeshAgent.destination = raycastHit.point;
        }
    }

    private void OnDestroy()
    {
       
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
