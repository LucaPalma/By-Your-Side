using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
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
    private bool pastTarget = false; 
    public TargetProjectile spawnProj;
    private Rigidbody rb;

    private void Awake()
	{
        rb = GetComponent<Rigidbody>();
	}

    public void Update()
    {
        //Display Draw Rays
        // They point to where the origin and initial target are
        Debug.DrawRay(transform.position, origin, Color.red);
        Debug.DrawRay(transform.position, targetSpot, Color.red);

        //Get Target Spot
        GetLocation(targetSpot);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }

        //If it has reached it's destination and has NOT already been to it's first target
        if (destinationDirection.magnitude < 1 && pastTarget == false)
        {
            LightningExplode();
            Destroy(this.gameObject);
            //Set target to be origin
            targetSpot = origin;
            pastTarget = true;
            //Set velocity to be towards new target and change rotation to fit with this
            GetComponent<Rigidbody>().velocity = targetSpot.normalized;
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized, Vector3.up);
        }
        //If it has reached it's destination and has already been to it's first target
        else if (destinationDirection.magnitude < 0.5f && pastTarget)
        {
            Destroy(this.gameObject);
        }
    }

    public void LightningExplode()
    {
        int localNum = 5;
        float windLifeTime = 0.3f;
        float windSpeed = 35;
        string projectileTarget = "Player";
        int windDamage = 7;
        int knockbackAmt = 5;
        

        float radius = 5f;
        float angleStep = 360f / localNum;
        float angle = 0f;

        for (int i = 0; i <= localNum; i++) 
        {

            float directionX = Mathf.Sin ((angle * Mathf.PI) / 180) * radius;
            float directionZ = Mathf.Cos ((angle * Mathf.PI) / 180) * radius;

            Vector3 projectileVector = new Vector3 (directionX, 0, directionZ);
            Vector3 projectileMoveDirection = (projectileVector).normalized * windSpeed;

            var projectile = Instantiate(spawnProj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.Euler(0, angle, 0));
            
            projectile.GetComponent<Rigidbody>().velocity = new Vector3 (projectileMoveDirection.x, 0, projectileMoveDirection.z);
            projectile.lifeTime = windLifeTime;
            projectile.speed = windSpeed;
            projectile.target = projectileTarget;
            projectile.damage = windDamage;
            projectile.knockback = knockbackAmt;

            angle += angleStep;
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
