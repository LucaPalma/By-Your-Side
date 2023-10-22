using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TornadoEnemy : BaseEnemy
{
    //[SerializeField] private float minDistToPlayer = 1f;
    //[SerializeField] private float maxDistToPlayer = 1f;
    //[SerializeField] protected float dmg = 3;
    //[SerializeField] private float pwr = 5;

    //protected iFrameHealth plrHealth;
    //public GameObject hurtBox;

    //[SerializeField] private string soundName;
	//private AudioSource abilitySound;
    //[SerializeField] private string deathName;
	//private AudioSource deathSound;

    private void Awake()
    {
        DummyHealth = GetComponent<Dummy>();
        startLocation = transform.position;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;

        lastSeenPosition = transform.position;

        //plrHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<iFrameHealth>();
        //abilitySound = GameObject.Find(soundName).GetComponent<AudioSource>();
        //deathSound = GameObject.Find(deathName).GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    protected override void Update()
    {
        GetPlayerLocation(); //See BaseEnemy parent code

        CanSeePlayer();     //See BaseEnemy parent code

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

    public override IEnumerator Attack()
    {
        canAttack = false;
        yield return null;
        canAttack = true;
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(lastSeenPosition, transform.position);
    }

    public void DeathSound()
    {
        //deathSound.Play();
    }
}
