using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBackProjectile1 : MonoBehaviour
{
    [Header("Projectile Variables")]
    public float lifeTime;
    public float damage;
    public float knockBack;
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
        if (collision.tag == target || collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                var knockable = collision.gameObject.GetComponent<Rigidbody>();
                if (collision.gameObject.GetComponent<BaseEnemy>() != null)
                {
                    var enemy = collision.gameObject.GetComponent<BaseEnemy>();
                    enemy.StartCoroutine("haltKnockback");
                    var enemynav = collision.gameObject.GetComponent<NavMeshAgent>();
                    enemynav.isStopped = true;
                    enemynav.speed = 0;
                    var dummy = collision.gameObject.GetComponent<Dummy>();
                    dummy.handleDamage(damage);
                }
                if (!radialKnockback)
                {
                    knockable.velocity = knockBackDir * knockBack;
                }
                else
                {
                    knockable.velocity = (knockable.position - this.transform.position).normalized * knockBack;
                }

            }
        }
        //Destroy enemy projectiles that are targeting the player.
        if (collision.tag == "Projectile" && collision.GetComponent<BasicProjectile>() !=null)
        {
            if (collision.GetComponent<BasicProjectile>().target == "Player")
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
