using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    public int camHeight = 10;
    Player player;
    Vector3 targetPos;
    float horiOffset = 0;
    float vertOffset = 0;
    public int offsetAmt = 2;
    public float cameraSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        horiOffset = Input.GetAxis("Horizontal"); //A and D scale from -1 to 1
        vertOffset = Input.GetAxis("Vertical"); //W and S scale from -1 to 1
        Vector3 updatedPos = new Vector3(player.transform.position.x + (horiOffset*offsetAmt),camHeight,player.transform.position.z + (vertOffset * offsetAmt));
        targetPos = updatedPos;
        transform.position = Vector3.MoveTowards(transform.position,targetPos,cameraSpeed);
        transform.position = new Vector3(transform.position.x,camHeight,transform.position.z);
        */
        transform.position = new Vector3(player.transform.position.x, camHeight, player.transform.position.z);
    }
}
