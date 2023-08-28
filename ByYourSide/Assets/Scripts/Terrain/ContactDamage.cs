using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public float damage;
    public float knockback;
    public bool trigger;



    private void OnCollisionEnter(Collision collision)
    {
        if (!trigger)
        {
            if (collision.collider.tag == "Player")
            {
                if (collision.gameObject.GetComponent<Rigidbody>() != null)
                {
                    var knockable = collision.gameObject.GetComponent<iKnockBackable>();
                    knockable.handleKnockBack(knockback, this.transform.position);

                    var damageable = collision.gameObject.GetComponent<iDamageable>();
                    damageable.handleDamage(damage);
                }
            }
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (trigger)
        {
            if (collision.tag == "Player")
            {
                if (collision.gameObject.GetComponent<Rigidbody>() != null)
                {
                    var knockable = collision.gameObject.GetComponent<iKnockBackable>();
                    knockable.handleKnockBack(knockback, this.transform.position);

                    var damageable = collision.gameObject.GetComponent<iDamageable>();
                    damageable.handleDamage(damage);
                }
            }
        }

    }



}
