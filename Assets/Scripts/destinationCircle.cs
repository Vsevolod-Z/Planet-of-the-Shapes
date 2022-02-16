using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destinationCircle : MonoBehaviour
{
    Vector3 pool_Position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator EndLife()
    {
        yield return new WaitForSeconds(1f);
        transform.position = pool_Position;
    }
}
