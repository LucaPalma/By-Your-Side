using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockBackProjectile1 : MonoBehaviour
{
    [Header("Projectile Variables")]
    public float lifeTime;
    public float damage;
    public float knockback;
    public float speed;
    public string target;
    public Vector3 knockBackDir;
    public bool radialKnockback; //Knock away from center of projectile instead of passing direction.

    public void Update()
    {
        //Destroy projectiles that fly off screen
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void FixedUpdate()
    {
        var rb = this.GetComponent<Rigidbody>();
        rb.velocity = rb.velocity.normalized * speed; //Continue in current direction.
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == target || collision.gameObject.layer == 7 || collision.gameObject.layer == 9)
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
        }
        //Destroy enemy projectiles that are targeting the player.
        if (collision.tag == "Projectile" && collision.GetComponent<BasicProjectile>() !=null)
        {
            if (collision.GetComponent<BasicProjectile>().target == "Enemy")
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
