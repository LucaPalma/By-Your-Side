using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    //Component Declaration
    protected Rigidbody2D rb;

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

    //Enemy Values
    [SerializeField] protected float fireRate = 2.0f;
    protected float moveSpeed;
    [SerializeField] private float persueDistance = 17.5f;

    [SerializeField] private LayerMask barrierLayer;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    private void Awake()
    {
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

    public void CanSeePlayer()
    {
        

        //RaycastHit2D hitObject = Physics2D.Raycast(transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, barrierLayer);

        var ray = new Ray(transform.position, directionToPlayer);
        RaycastHit hit;
        rayLength = directionToPlayer.magnitude;
        if (rayLength > persueDistance) //If the player leaves the persue distance, the enemies will go to the place the player was last
        {                               //seen that was inside the persue distance
            rayLength = persueDistance;
        }
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);
        if (Physics.Raycast(ray, out hit, rayLength, barrierLayer) && !(hit.transform.gameObject.layer == 9 ))
            {
                seenPlayer = true;
                playerInLOS = true;
                lastSeenPosition = playerPosition;
            }
        else {playerInLOS = false;}
        

        //RaycastHit hitObject = Physics.Raycast(transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, barrierLayer);

        //playerInLOS = hitObject.collider == null;
//
        //// If the player is detected, the corresponding object state is updated.
        //if (playerInLOS)
        //{
        //    seenPlayer = true;
        //    lastSeenPosition = playerPosition;
        //}
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
        ////If no path exists or the enemy cannot move, exit the function
        //if (currentPath == null || canMove == false)
        //{
        //    return;
        //}
        //
        ////Determines whether or not we have reached the end of the path
        //if (currentWaypoint >= currentPath.vectorPath.Count)
        //{
        //    return;
        //}

        ////Calculates the direction and force in which to move the enemy
        //Vector2 direction = ((Vector2)currentPath.vectorPath[currentWaypoint] - rb.position).normalized;
        //Vector2 force = direction * moveSpeed;
//
        //rb.AddForce(force);
//
        //float distance = Vector2.Distance(rb.position, currentPath.vectorPath[currentWaypoint]);
//
        //if (distance < nextWaypointDistance)
        //{
        //    currentWaypoint++;
        //}
    }
}
