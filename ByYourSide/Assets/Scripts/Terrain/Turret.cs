using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //References
    Rigidbody rb;

    [Header("Turret Stats")]
    public BasicProjectile proj;
    public float projectileLifeTime;
    public float projectileDamage;
    public float projectileSpeed;
    public float projectileKnockback;
    public string projectileTarget;

    public float fireRate;
    public float fireCD;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireCD <= 0)
        {
            Shoot();
            fireCD = fireRate;
        }

        fireCD -= Time.deltaTime;
    }

    void Shoot()
    {
        var projectile = Instantiate(proj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = this.transform.forward.normalized * projectileSpeed;

        projectile.lifeTime = projectileLifeTime;
        projectile.damage = projectileDamage;
        projectile.speed = projectileSpeed;
        projectile.knockback = projectileKnockback;
        projectile.target = projectileTarget;
    }
}
