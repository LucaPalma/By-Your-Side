using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinningScript : MonoBehaviour
{
    Transform t;
    public int rotationAmt = 1;

    void Start()
    {
        t = GetComponent<Transform>();
    }


    void Update()
    {
        t.Rotate(0, 0, rotationAmt*Time.deltaTime);
    }
}
