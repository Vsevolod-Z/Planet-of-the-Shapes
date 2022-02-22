using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    [SerializeField]
    Camera playerCamera;
    [SerializeField]
    private float screenWidth;
    [SerializeField]
    private float screenHeight;
    [SerializeField]
    private int offSet = 30;
    [SerializeField]
    private float speed = 70;
    private Vector3  playerObjectPosition;
    [SerializeField]
    private GameObject selectionUnitsSystemObjectPrefab;
    // Start is called before the first frame update
    public Camera PlayerCamera
    {
        get
        {
            return playerCamera;
        }
    }
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width ;
        playerCamera = GetComponentInChildren<Camera>();
        selectionUnitsSystemObjectPrefab.GetComponent<UnitsSelectionComponent>().playerCamera = playerCamera;
        Instantiate(selectionUnitsSystemObjectPrefab);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        playerObjectPosition = transform.position;
        Debug.Log(Input.mousePosition);
        if (Input.mousePosition.x > 0 && Input.mousePosition.x < screenWidth && Input.mousePosition.y > 0 && Input.mousePosition.y < screenHeight)
        {
            if (Math.Abs(Input.mousePosition.x) >= screenWidth - offSet)
            {
                playerObjectPosition.x += Time.deltaTime * speed;
            }
            if (Math.Abs(Input.mousePosition.x) <= offSet)
            {
                playerObjectPosition.x -= Time.deltaTime * speed;
            }
            if (Math.Abs(Input.mousePosition.y) >= screenHeight - offSet)
            {
                playerObjectPosition.z += Time.deltaTime * speed;
            }
            if (Math.Abs(Input.mousePosition.y) <= offSet)
            {
                playerObjectPosition.z -= Time.deltaTime * speed;
            }
        
            if(playerCamera.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 15 < 80 && playerCamera.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 15 > 40)
            playerCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel")*15;
        }
        transform.position = playerObjectPosition;
    }
}
