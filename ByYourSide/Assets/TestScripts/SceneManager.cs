using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Linq;


public class SceneMan : MonoBehaviour
{
    NavMeshSurface[] surface;
    public NavMeshSurface surfaceSingle;

    //public Transform objectToRotate;
    // Start is called before the first frame update
    void Start()
    {
        surface = FindObjectsOfType<NavMeshSurface>();
        UpdatePath();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        //objectToRotate.localRotation = Quaternion.Euler(new Vector3(0, 15 * Time.deltaTime, 0) + objectToRotate.localRotation.eulerAngles);
        //surface.BuildNavMesh();
    }

    public void UpdatePath()
    {
        surfaceSingle.BuildNavMesh();
        //foreach (NavMeshSurface n in surface)
        //{
        //    n.BuildNavMesh();
        //}

    }
}
