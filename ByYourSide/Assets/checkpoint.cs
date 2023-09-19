using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("triggered check");
            collision.GetComponent<Player>().currentCheckpoint = new Vector3(transform.position.x,1.14f,transform.position.z);
        }
    }




}
