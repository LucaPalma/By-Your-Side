using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //References
    public Rigidbody rb;
    Pet pet;

    [Header("Movement Stats")]
    public float moveSpeed;
    float intentionX;
    float intentionY;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pet = FindObjectOfType<Pet>();
    }

    void Update()
    {
        handleInput();
        handleMovement();


    }

    public void handleInput()
    {
        intentionX = Input.GetAxis("Horizontal"); //A and D scale from -1 to 1
        intentionY = Input.GetAxis("Vertical"); //W and S scale from -1 to 1

    }

    public void handleMovement()
    {
        //Update playres velocity based on intention.
        var newVel = new Vector3((moveSpeed * Time.deltaTime * 100) * intentionX, rb.velocity.y, (moveSpeed * Time.deltaTime * 100) * intentionY);
        rb.velocity = newVel;
        pet.rb.velocity = newVel;


    }

}
