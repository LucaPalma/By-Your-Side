using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectile : MonoBehaviour
{
    [Header("Projectile Variables")]
    public float lifeTime;
    public float damage;
    public float knockback;
    public float speed;
    public string target;
    public Vector3 origin;
    public Vector3 targetSpot;
    public Vector3 destinationDirection;
    public Vector3 targetLocation;
    private bool pastTarget = false; 
    private Transform player;

    [Header("Sounds")]
    [SerializeField] private string lightningName = "BossLightning";
	private AudioSource lightningSound;
    [SerializeField] private string lightningQuietName = "BossStrike";
	private AudioSource lightningQuietSound;

    private void Awake()
	{
        lightningSound = GameObject.Find(lightningName).GetComponent<AudioSource>();
        lightningQuietSound = GameObject.Find(lightningQuietName).GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

    private void Start()
	{
        lightningSound.Play();
	}

    public void Update()
    {
        //Get Target Spot
        GetLocation(targetSpot);
        lifeTime -= Time.deltaTime;
        // After the bolts have travelled a distance away from their creation point
        // Change direction to go stright towards the playter's point
        if (lifeTime <= 0 && pastTarget == false)
        {
            lightningQuietSound.Play();
            targetLocation = player.position;
            //Set velocity to be towards new target and change rotation to fit with this
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().AddForce(targetLocation - transform.position);
		    transform.rotation = Quaternion.LookRotation((targetLocation - transform.position).normalized);

            pastTarget = true;
            lifeTime = 20;
        }
        else if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void GetLocation(Vector3 destination)
    {
        destinationDirection = destination - transform.position;
    }

    public void FixedUpdate()
    {
        var rb = this.GetComponent<Rigidbody>();
        rb.velocity = rb.velocity.normalized * speed; //Continue in current direction.
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == target)
        {
            if (collision.gameObject.GetComponent<iKnockBackable>() != null)
            {
                var damageable = collision.gameObject.GetComponent<iKnockBackable>();
                damageable.handleKnockBack(knockback, this.transform.position);
            }
            if (collision.gameObject.GetComponent<iDamageable>() != null)
            {
                var damageable = collision.gameObject.GetComponent<iDamageable>();
                damageable.handleDamage(damage);
            }

            //Don't destroy projectiles while dodging.
            var p = collision.gameObject.GetComponent<Player>();
            if (collision.gameObject.tag == "Player")
            {
                if (!(p.dashInvuln))
                {
                    Destroy(this.gameObject);
                }          
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.layer == 8|| collision.gameObject.layer == 9) //8 Is terrain layer.  9 is obstacle layer
        {
            Destroy(this.gameObject); //Destroys object on collision with terrain;
        }
    }

}
