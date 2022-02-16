using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSelectionComponent : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMaskForSelect;
    [SerializeField]
    LayerMask layerMaskForRays;
    static Texture2D _whiteTexture;
    bool dragSelect;
    RaycastHit hit;
    // ---------------------------------------Units Dictionares----------------------------------------
    [SerializeField]
    public Dictionary<int, GameObject> selectedUnits = new Dictionary<int, GameObject>();
    [SerializeField]
    public Dictionary<int, GameObject> AllUnits = new Dictionary<int, GameObject>();
    //----------------------------------------------------------------------------------------------------

    //----- Mouse Position Vectors--------
    [SerializeField]
    private Vector3 startSelectionPosition;
    [SerializeField]
    private Vector3 endSelectionPosition;
    //-----------------------------------------
    MeshCollider selectionBox;
    Mesh selectionMesh;
    //the corners of our 2d selection box
    [SerializeField]
    Vector2[] corners;

    //the vertices of our meshcollider
    [SerializeField]
    Vector3[] verts = new Vector3[4];
    [SerializeField]
    Vector3[] vecs = new Vector3[4];

    private void Start()
    {
        dragSelect = false;
    }
    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedUnits.ContainsKey(id)))
        {
            selectedUnits.Add(id, go);
            go.AddComponent<selectedUnitComponent>();
            Debug.Log("Added" + id + "to selected Dictionary");
        }
    }
    public void DeSelect(int id)
    {
        Destroy(selectedUnits[id].GetComponent<UnitsSelectionComponent>());
        selectedUnits.Remove(id);
    }

    public void DeSelectAll()
    {
        foreach ( KeyValuePair<int,GameObject> pair in selectedUnits)
        {
            if(pair.Value != null)
            {
                Destroy(selectedUnits[pair.Key].GetComponent<selectedUnitComponent>());
            }
        }
        selectedUnits.Clear();
    }


    private void Update()
    {
        //1. when left mouse button clicked (but not released)
        if (Input.GetMouseButtonDown(0))
        {
            startSelectionPosition = Input.mousePosition;
        }

        //2. while left mouse button held
        if (Input.GetMouseButton(0))
        {
            if ((startSelectionPosition - Input.mousePosition).magnitude > 20)
            {
                dragSelect = true;
            }
        }

        //3. when mouse button comes up
        if (Input.GetMouseButtonUp(0))
        {
            if (dragSelect == false) //single select
            {
                Ray ray = Camera.main.ScreenPointToRay(startSelectionPosition);

                if (Physics.Raycast(ray, out hit, 50000.0f, layerMaskForSelect))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                    {
                        AddSelected(hit.transform.gameObject);
                    }
                    else //exclusive selected
                    {
                        DeSelectAll();
                        AddSelected(hit.transform.gameObject);
                    }
                }
                else //if we didnt hit something
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //do nothing
                    }
                    else
                    {
                        DeSelectAll();
                    }
                }
            }
            else //marquee select
            {
                int i = 0;
                endSelectionPosition = Input.mousePosition;
                corners = getBoundingBox(startSelectionPosition,endSelectionPosition);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, 50000.0f,layerMaskForRays))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point ;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;
                
               
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeSelectAll();
                }

                Destroy(selectionBox, 0.02f);

            }//end marquee select

            dragSelect = false;

        }

    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(startSelectionPosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }

    //generate a mesh from the 4 bottom points
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

   private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("Попал в триггер: " + other.gameObject.name);
            AddSelected(other.gameObject);
        }

    }
}
