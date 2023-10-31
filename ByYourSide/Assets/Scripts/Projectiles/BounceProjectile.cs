using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceProjectile : MonoBehaviour
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

    private void Start()
	{
        //origin = transform.position;
	}

    public void Update()
    {
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
            //Set target to be origin
            targetSpot = origin;
            pastTarget = true;
            //Set velocity to be towards new target and change rotation to fit with this
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().AddForce(targetSpot - transform.position);
		    transform.rotation = Quaternion.LookRotation((targetSpot - transform.position).normalized);
        }
        //If it has reached it's destination and has already been to it's first target
        else if (destinationDirection.magnitude < 0.5f && pastTarget)
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
