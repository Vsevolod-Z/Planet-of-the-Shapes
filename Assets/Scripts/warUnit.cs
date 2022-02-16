using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class warUnit : MonoBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    Vector3 mousePosition;
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        mainCamera = Camera.main;

    }


    // Update is called once per frame
    void Update()
    {
    
    }
    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = transform.position;
    }
    
}

