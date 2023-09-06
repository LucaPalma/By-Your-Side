using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    //[SerializeField]
    //private GameObject projectilePrefab; // the projectile that this enemy fires at the player
    //Rigidbody rb;
    [SerializeField]
    private Transform entity; // the reference to this enemy
    [SerializeField]
    private float shootDistance = 12.0f;
    [SerializeField]
    private float slowDistance = 10.0f;
    //[SerializeField] private string soundName;
	//private AudioSource abilitySound;
    //[SerializeField] private string deathName;
	//private AudioSource deathSound;


    [Header("Shooting Stats")]
    [SerializeField] private BasicProjectile proj;
    [SerializeField] private float projectileLifeTime;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileKnockback;
    [SerializeField] private string projectileTarget;

    [SerializeField] private float fireCD;

    private void Awake()
	{
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        moveSpeed = agent.speed;

        lastSeenPosition = transform.position;

        oldMoveSpeed = moveSpeed;
        canMove = true;
        //abilitySound = GameObject.Find(soundName).GetComponent<AudioSource>();
        //deathSound = GameObject.Find(deathName).GetComponent<AudioSource>();
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
            //moveSpeed = oldMoveSpeed/10;
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
        if (canMove)
        {
            Move();
        }

        if (directionToPlayer.magnitude < slowDistance) //If player is inside slow distance
        {
            agent.speed = oldMoveSpeed/10; //Slow speed by 90%
        }
        else {agent.speed = oldMoveSpeed;} //Reset speed
    }

    public override IEnumerator Attack()
	{
        canAttack = false;

        // Luca's turret Code
        var projectile = Instantiate(proj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = this.transform.forward.normalized * projectileSpeed;

        projectile.lifeTime = projectileLifeTime;
        projectile.damage = projectileDamage;
        projectile.speed = projectileSpeed;
        projectile.knockback = projectileKnockback;
        projectile.target = projectileTarget;

        // wait amount of seconds before firing again
        yield return new WaitForSeconds(fireRate);
        canAttack = true;
    }

    public void DeathSound()
    {
        //deathSound.Play();
    }
}
