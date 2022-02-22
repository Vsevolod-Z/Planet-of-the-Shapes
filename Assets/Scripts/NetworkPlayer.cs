using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField]
    bool isLocal;
    [SerializeField]
    private GameObject playerCamera;
    private void Start()
    {
        isLocal = GetComponent<NetworkObject>().IsLocalPlayer;
        if (!GetComponent<NetworkObject>().IsLocalPlayer)
        {
            playerCamera.SetActive(false);
            if (GetComponent<UnitsSelectionComponent>())
            {
                GetComponent<UnitsSelectionComponent>().enabled = false;
            }
            if(GetComponent<CameraMovment>())
            {
                GetComponent<CameraMovment>().enabled = false;
            }
        }
    }
}
