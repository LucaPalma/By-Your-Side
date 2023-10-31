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

    private void Awake()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //origin = transform.position;
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
        if (lifeTime <= 0 && pastTarget == false)
        {
            targetLocation = player.position;
            //player.position;
            //Set velocity to be towards new target and change rotation to fit with this
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            //GetComponent<Rigidbody>().velocity = new Vector3(0, 20, 0);
            GetComponent<Rigidbody>().AddForce(targetLocation - transform.position);
            //GetComponent<Rigidbody>().velocity = player.position;

            //find the vector pointing from our position to the target
		    //_direction = (targetLocation - transform.position).normalized;

		    //create the rotation we need to be in to look at the target
		    transform.rotation = Quaternion.LookRotation((targetLocation - transform.position).normalized);

            //rotate us over time according to speed until we are in the required rotation
		    //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

            //transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized, Vector3.up);
            pastTarget = true;
            lifeTime = 20;
        }
        else if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }

        if (pastTarget == true)
        {
            
            //transform.position = Vector3.MoveTowards(transform.position, targetLocation, 0.1f);
        }

        

        ////If it has reached it's destination and has NOT already been to it's first target
        //if (destinationDirection.magnitude < 1 && pastTarget == false)
        //{
        //    //Set target to be origin
        //    targetSpot = origin;
        //    pastTarget = true;
        //    //Set velocity to be towards new target and change rotation to fit with this
        //    GetComponent<Rigidbody>().velocity = targetSpot.normalized;
        //    transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity.normalized, Vector3.up);
        //}
        ////If it has reached it's destination and has already been to it's first target
        //else if (destinationDirection.magnitude < 0.5f && pastTarget)
        //{
        //    Destroy(this.gameObject);
        //}

        

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
