using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamEnemy : BaseEnemy
{
    [SerializeField] private float minDistToPlayer = 1f;
    [SerializeField] private float maxDistToPlayer = 1f;
    [SerializeField] protected float dmg = 3;
    [SerializeField] private float pwr = 5;
    [SerializeField] private Animator anim;

    //protected iFrameHealth plrHealth;
    //public GameObject hurtBox;

    [Header("Sounds")]
    [SerializeField] private string shootName = "EnemyMelee";
	private AudioSource shootSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;

        lastSeenPosition = transform.position;

        shootSound = GameObject.Find(shootName).GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    protected override void Update()
    {
        GetPlayerLocation(); //See BaseEnemy parent code

        CanSeePlayer();     //See BaseEnemy parent code

        if (playerInLOS)
        {
            if (GetDistanceToPlayer() < maxDistToPlayer && canAttack && !(GetDistanceToPlayer() < minDistToPlayer))
            {
                StartCoroutine(Attack());
            }
            
            if (GetDistanceToPlayer() < maxDistToPlayer)
            {
                anim.SetBool("attacking", true);
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
    }

    public override IEnumerator Attack()
    {
        //Debug.Log("Pew");
        shootSound.Play();
        canMove = false;
        agent.ResetPath();
        canAttack = false;
        this.rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.4f);
        Vector3 dir = directionToPlayer;
        yield return new WaitForSeconds(0.1f);
        //hurtBox.GetComponent<BoxCollider2D>().enabled = true;
        dir.Normalize();
        var force = dir * pwr * directionToPlayer.magnitude;
        this.rb.AddForce(force, ForceMode.Impulse);
        //abilitySound.Play();
        yield return new WaitForSeconds(fireRate / 2);
        canMove = true;
        yield return new WaitForSeconds(2 / 2);
        //hurtBox.GetComponent<BoxCollider2D>().enabled = false;
        canAttack = true;
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(lastSeenPosition, transform.position);
    }
}
