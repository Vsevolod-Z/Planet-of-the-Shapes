using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRTSController : MonoBehaviour
{

    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Vector3 endPosition;
    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = GetMousePosition();
        } 
        if (Input.GetMouseButtonUp(0))
        {
            endPosition = GetMousePosition();
           //Collider[] colliderArray = Physics.overlap(startPosition, endPosition);
            
        }
        
    }

    Vector3 GetMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
           return raycastHit.point;
        }
        else
        {
            return new Vector3(0,0,0);
        }
    }
}
