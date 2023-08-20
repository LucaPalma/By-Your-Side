using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    public int camHeight = 10;
    Player player;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 updatedPos = new Vector3(player.transform.position.x,player.transform.position.y + camHeight,player.transform.position.z);
        targetPos = updatedPos;
        
        transform.position = targetPos;

    }
}
