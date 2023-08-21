using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Pet : MonoBehaviour
{
    //References
    Player player;
    public Rigidbody rb;
    MainCam cam;

    //Variables needed for mouse position detection
    public Vector3 worldPosition;
    Plane plane = new Plane(Vector3.up, 0);

    //Input detection bools
    bool wantToShoot;//Main attack
    bool wantToStrong;//Stronger Secondary
    bool wantToBuff; //Buff Main Adventurer

    [Header("Movement Stats")]
    public float moveSpeed;
    public float floatDist;//How far from the main player the pet can stray.
    Vector3 targetPos;

    [Header("Cooldowns")]
    public float mainAttackMaxCD;
    float mainAttackCurrentCD = 0;

    public float strongAttackMaxCD;
    float strongAttackCurrentCD = 0;

    public float buffAttackMaxCD;
    float buffAttackCurrentCD = 0;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<MainCam>();

    }
    
    private void Update()
    {
        getMousePosition(); //Assigns mouse to worldPosition variable.
        getInput();
        handlePosition();
        handleAttacks();
    }

    public void getInput()
    {
        if (Input.GetMouseButton(0)) //Can hold instead of press M1
        {
            wantToShoot = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            wantToStrong = true;
        }

        if(Input.GetKeyDown(KeyCode.Keypad7))
        {
            wantToBuff = true;
        }
    }

    public void handlePosition()
    {
        //update mouse position as vector3.
        var mousePos = new Vector3(worldPosition.x,player.transform.position.y,worldPosition.z);

        //Constrain target position to a certain distance from adventurer.
        var heading = mousePos - player.rb.position;//Get direction from player to mouse.
        heading = heading.normalized; //Normalise direction force.
        targetPos = (heading * floatDist) + player.rb.position; //Find point equal to float distance from player in target direction.

       //Move if distance to target is too high.
        if (Vector3.Distance(rb.position, targetPos) > 0.5)
        {
            rb.position = Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime); //Move this from current position to target position.
        }
    }

    public void handleAttacks()
    {
        if(wantToShoot && mainAttackCurrentCD <= 0)
        {
            mainAttack();
            wantToShoot = false;
            mainAttackCurrentCD = mainAttackMaxCD;
        }

        if(wantToStrong && strongAttackCurrentCD <= 0)
        {
            strongAttack();
            wantToStrong=false;
            strongAttackCurrentCD = strongAttackMaxCD;
        }

        if(wantToBuff && buffAttackCurrentCD <= 0)
        {
            buffAttack();
            wantToBuff = false;
            buffAttackCurrentCD = buffAttackMaxCD;
        }

        //Cooldown Updates
        mainAttackCurrentCD -= Time.deltaTime;
        strongAttackCurrentCD -= Time.deltaTime;
        //Reset Triggers
        wantToShoot = false;
        wantToStrong = false;

    }
     
    public virtual void mainAttack()
    {
        //Each pet will implement this uniquely.
    }


    public virtual void strongAttack()
    {
        //Each pet will implement this uniquely.
    }

    public virtual void buffAttack()
    {
        //Each pet will implement this uniquely.
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
