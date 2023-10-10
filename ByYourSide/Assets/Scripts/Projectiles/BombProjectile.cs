using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    Rigidbody rb;

    [Header("Projectile Variables")]
    public float lifeTime;
    public float damage;
    public float knockback;
    public float speed;
    public string target;

    [Header("Explosion Variables")]
    public KnockBackProjectile windProj;
    public float windLifeTime;
    public float windDamage;
    public float windKnockBack;
    public float windSpeed;
    public string windTarget;

    [Header("Sounds")]
    [SerializeField] private string explodeName = "ExplodeQuiet";
	private AudioSource explodeSound;

    private void Awake()
	{
        explodeSound = GameObject.Find(explodeName).GetComponent<AudioSource>();
	}

    public void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        //Destroy projectiles that fly off screen
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Explode();
            Destroy(this.gameObject);
        }
    }

    public void FixedUpdate()
    {
        var rb = this.GetComponent<Rigidbody>();
        rb.velocity = rb.velocity.normalized * speed; //Continue in current direction.
    }

    public void Explode()
    {
        explodeSound.Play();
        var projectile = Instantiate(windProj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
        projectile.lifeTime = windLifeTime;
        projectile.damage = windDamage;
        projectile.speed = windSpeed;
        projectile.target = target;
        projectile.knockBack = windKnockBack;
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
                    Explode();
                    Destroy(this.gameObject);
                }          
            }
            else
            {
                Explode();
                Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.layer == 8|| collision.gameObject.layer == 9) //8 Is terrain layer.  9 is obstacle layer
        {
            Explode();
            Destroy(this.gameObject); //Destroys object on collision with terrain;
        }
    }

}
