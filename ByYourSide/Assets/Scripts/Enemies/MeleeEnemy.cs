using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private float minDistToPlayer = 0f;
    [SerializeField] protected float dmg = 3;
    [SerializeField] private float pwr = 5;
    [SerializeField] private Animator anim;
    [SerializeField]
    private float slowDistance = 10.0f;

    private void Awake()
    {
        DummyHealth = GetComponent<Dummy>();
        startLocation = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;

        oldMoveSpeed = moveSpeed;

        lastSeenPosition = transform.position;
        
    }
    

    // Update is called once per frame
    protected override void Update()
    {
        GetPlayerLocation(); //See BaseEnemy parent code

        CanSeePlayer();     //See BaseEnemy parent code

        if (playerInLOS)
        {
            if (GetDistanceToPlayer() < minDistToPlayer && canAttack)
            {
                anim.SetBool("attacking", true);
                StartCoroutine(Attack());
            }
            else 
            {
                anim.SetBool("attacking", false);
            }
        }

        if (canMove)
        {
            Move();
        }

        if (directionToPlayer.magnitude < slowDistance && playerInLOS) //If player is inside slow distance
        {
            agent.speed = oldMoveSpeed*1.2f; //increase speed by 20%
        }
        else if (directionToPlayer.magnitude < (slowDistance/2) && playerInLOS) //If player is inside slow distance
        {
            agent.speed = oldMoveSpeed*1.5f; //increase speed by 50%
        }
        else {agent.speed = oldMoveSpeed;} //Reset speed
    }

    public override IEnumerator Attack()
    {
        //canMove = false;
        //canAttack = false;
        //rb.velocity = Vector3.zero;
        //yield return new WaitForSeconds(0.4f);
        //Vector2 dir = directionToPlayer;
        //yield return new WaitForSeconds(0.1f);
        //hurtBox.GetComponent<BoxCollider2D>().enabled = true;
        //dir.Normalize();
        //var force = dir * pwr;
        //rb.AddForce(force, ForceMode2D.Impulse);
        //abilitySound.Play();
        //yield return new WaitForSeconds(fireRate / 2);
        //canMove = true;
        yield return new WaitForSeconds(2 / 2);
        //hurtBox.GetComponent<BoxCollider2D>().enabled = false;
        //canAttack = true;
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(lastSeenPosition, transform.position);
    }
}
