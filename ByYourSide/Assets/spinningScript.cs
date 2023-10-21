using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinningScript : MonoBehaviour
{
    Transform t;
    public int rotationAmt = 1;

    //idk why but the pet rotates on a different axis
    public bool pet = false;

    void Start()
    {
        t = GetComponent<Transform>();
    }


    void Update()
    {
        if (!pet)
        {
            t.Rotate(0, 0, rotationAmt * Time.deltaTime);
        }
        else t.Rotate(0, rotationAmt * Time.deltaTime, 0);

    }
}
