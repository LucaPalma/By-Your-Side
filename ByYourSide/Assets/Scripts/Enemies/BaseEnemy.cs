using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    //Component Declaration
    protected Rigidbody rb;
    

    //Player Information
    protected Transform player;
    protected Vector3 playerPosition;
    protected Vector2 playerPosition2D;
    protected Vector3 directionToPlayer;
    protected Vector3 lastSeenPosition;
    protected float rayLength;

    //Object States Declaration
    protected bool playerInLOS = false;
    protected bool seenPlayer = false; //If enemy has seen player at any point
    public bool canAttack = true;
    public bool canMove = true;  

    //Reset enemy Variables
    protected Vector3 startLocation;
    protected Dummy DummyHealth;

    //Enemy Values
    [SerializeField] protected float fireRate = 2.0f;
    public float moveSpeed;
    [SerializeField] protected float persueDistance = 17.5f;

    [SerializeField] protected LayerMask barrierLayer;

    protected NavMeshAgent agent;

    protected float oldMoveSpeed;

    IEnumerator haltKnockback()
    {
        canMove = false;
        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);//Cuts the velocity of the object.
        canMove = true;
        agent.speed = moveSpeed;
        agent.isStopped = false;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        DummyHealth = GetComponent<Dummy>();
        startLocation = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;

        lastSeenPosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Update the player's position as they move
        GetPlayerLocation();
        //Determine whether the enemy can see the player

        CanSeePlayer();
        // If the enemy has not seen the player yet remain inactive
        if (!seenPlayer) return;
        //If the enemy can see the player move towards them and shoot
        if (playerInLOS)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        if (canMove)
        {
            Move();
        }
    }

    //Updates the player's location and direction from the enemy.
    public void GetPlayerLocation()
    {
        playerPosition = player.position;
        playerPosition2D = player.position;
        directionToPlayer = playerPosition - transform.position;
    }

    public virtual void CanSeePlayer()
    {
        var ray = new Ray(transform.position, directionToPlayer);
        RaycastHit hit;
        rayLength = directionToPlayer.magnitude;
        if (rayLength > persueDistance) //If the player leaves the persue distance, the enemies will go to the place the player was last
        {                               //seen that was inside the persue distance
            rayLength = persueDistance;
        }
        //Debug.DrawRay(transform.position, directionToPlayer, Color.red);
        if (Physics.Raycast(ray, out hit, rayLength, barrierLayer) && !(hit.transform.gameObject.layer == 9 )) //If Ray hits object AND object is of player player (9)
            {
                seenPlayer = true;
                playerInLOS = true;
                lastSeenPosition = playerPosition;
            }
        else {playerInLOS = false;}
        
    }

    //Fires a projectile at the player if the player is in line of sight.
    public virtual IEnumerator Attack()
    {
        canAttack = false;
        Debug.Log("Pew");
        //Create instance of enemy projectile
        //yield return new WaitWithPause(fireRate);  // This was here so the game can be paused, which was never used anyway
        yield return null;
        canAttack = true;
    }

    //Attempts to move the enemy towards the player
    public void Move()
    {
        agent.destination = lastSeenPosition;
    }

    public virtual void ResetEnemy()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.position = startLocation;
        lastSeenPosition = startLocation;
        
        //Debug.Log(DummyHealth.health);
        DummyHealth.resetHealth();
        this.gameObject.SetActive(true);
        
    }
}
