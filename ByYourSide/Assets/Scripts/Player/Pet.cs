using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Pet : MonoBehaviour
{
    //References
    Player player;
    Rigidbody rb;
    MainCam cam;

    //Variables needed for mouse
    public Vector3 worldPosition;
    Plane plane = new Plane(Vector3.up, 0);

    [Header("Movement Stats")]
    public float moveSpeed;
    public float floatDist;//How far from the main player the pet can stray.
    Vector3 targetPos;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<MainCam>();

    }
    
    private void Update()
    {
        getMousePosition();
        handlePosition();
    }

    public void handlePosition()
    {
        //update mouse position as vector3.
        var mousePos = new Vector3(worldPosition.x,player.transform.position.y,worldPosition.z);

        //Constrain target position to a certain distance from adventurer.
        var heading = player.rb.position - mousePos;//Get direction from player to mouse.
        

        //Move this from current position to target position.
        rb.position = Vector3.MoveTowards(rb.position, targetPos, moveSpeed*Time.deltaTime);
    }

    public void getMousePosition()
    {
        float distance = 100;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
    }



}
