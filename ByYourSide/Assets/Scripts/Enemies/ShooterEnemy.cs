using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    [SerializeField]
    private Transform entity; // the reference to this enemy
    [SerializeField]
    private float shootDistance = 12.0f;
    [SerializeField]
    private float slowDistance = 10.0f;
    [SerializeField] private Animator anim;



    [Header("Shooting Stats")]
    [SerializeField] private BasicProjectile proj;
    [SerializeField] private float projectileLifeTime;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileKnockback;
    [SerializeField] private string projectileTarget;

    [SerializeField] private float fireCD;

    [Header("Sounds")]
    [SerializeField] private string shootName = "EnemyShoot";
	private AudioSource shootSound;

    private void Awake()
	{
        DummyHealth = GetComponent<Dummy>();
        startLocation = transform.position;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        moveSpeed = agent.speed;

        lastSeenPosition = transform.position;

        oldMoveSpeed = moveSpeed;
        canMove = true;
        shootSound = GameObject.Find(shootName).GetComponent<AudioSource>();
	}

	protected override void Update()
	{
        //Update the player's position as they move
        GetPlayerLocation();
        //Determine whether the enemy can see the player
        CanSeePlayer();
        // if the enemy can see the player, shoot at them
        if (playerInLOS && directionToPlayer.magnitude <= shootDistance)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        // if the enemy can't see the player, move towards them
        else
        {
            moveSpeed = oldMoveSpeed;
        }

        if (playerInLOS)
        {
            anim.SetBool("attacking", true);
        }
        else 
        {
            anim.SetBool("attacking", false);
        }

        if (canMove)
        {
            Move();
        }

        // Also if the player is within a range and in line of sight, slow down.
        if (directionToPlayer.magnitude < slowDistance && playerInLOS) //If player is inside slow distance
        {
            agent.speed = oldMoveSpeed/10; //Slow speed by 90%
        }
        else {agent.speed = oldMoveSpeed;} //Reset speed
    }

    // This basically just makes a bullet with the correct properties, 
    // then waits the fire rate time, then makes another bullet again. 
    public override IEnumerator Attack()
	{
        canAttack = false;

        shootSound.Play();

        // Luca's turret Code
        var projectile = Instantiate(proj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = directionToPlayer * projectileSpeed;

        projectile.lifeTime = projectileLifeTime;
        projectile.damage = projectileDamage;
        projectile.speed = projectileSpeed;
        projectile.knockback = projectileKnockback;
        projectile.target = projectileTarget;

        // wait amount of seconds before firing again
        yield return new WaitForSeconds(fireRate);
        canAttack = true;
    }
}
