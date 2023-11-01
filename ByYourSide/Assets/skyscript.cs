using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyscript : MonoBehaviour
{
    public float state = 0;

    void Update()
    {
        if (state < 1000)
        {
        transform.position = new Vector3(this.transform.position.x, transform.position.y, transform.position.z + 0.03f);
        }
        else
        {
            transform.position = new Vector3(this.transform.position.x, transform.position.y, transform.position.z - 0.03f);
        }
        if (state > 2000)
        {
            state = 0;
        }
        state += 0.5f;
    }
}
