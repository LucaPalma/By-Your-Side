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

    private void Awake()
	{
        //GetLocation(targetSpot);
	}

    public void Update()
    {
        //Destroy projectiles that fly off screen
        GetLocation(targetSpot);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }

        if (destinationDirection.magnitude < 1 && pastTarget == false)
        {
            //Destroy(this.gameObject);

            targetSpot = origin;
            pastTarget = true;
            GetComponent<Rigidbody>().velocity = origin.normalized;
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized, Vector3.up);
        }
        else if (destinationDirection.magnitude < 1 && pastTarget)
        {
            Destroy(this.gameObject);
        }

        Debug.DrawRay(transform.position, origin, Color.red);
        Debug.DrawRay(transform.position, targetSpot, Color.red);

        //GetLocation();
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
