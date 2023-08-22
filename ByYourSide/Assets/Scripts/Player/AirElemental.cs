using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirElemental : Pet
{
    [Header("Air Elemental Basic Attack Variables")]
    public BasicProjectile proj;
    public float projectileLifeTime;
    public float projectileDamage;
    public float projectileSpeed;
    public string projectileTarget;

    [Header("Air Elemental Strong Attack Variables")]
    public KnockBackProjectile windProj;
    public float windLifeTime;
    public float windDamage;
    public float knockbackAmt;
    public float windSpeed;

    [Header("Air Elemental Buff Variables")]
    public float moveSpeedBuff;
    public float moveSpeedDuration;
    float currentMoveSpeedDuration;
    bool speedy;//Active while player is buffed.

    public override void Update()
    {
        base.Update();//Update functionality of pet.

        currentMoveSpeedDuration -= Time.deltaTime;
        if (currentMoveSpeedDuration <= 0 && speedy)
        {
            speedy = false;
            player.moveSpeed -= moveSpeedBuff;
        }
    }

    public override void mainAttack()
    {
        var mousePos = new Vector3(worldPosition.x, player.transform.position.y, worldPosition.z); // Get Mouse Position
        var heading = new Vector3(mousePos.x,this.rb.position.y,mousePos.z) - new Vector3(this.rb.position.x,this.rb.position.y,this.rb.position.z);//Get direction from pet to mouse.
        heading = heading.normalized; //Normalise direction force.
        var projectile = Instantiate(proj,new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z),Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = heading;

        projectile.lifeTime = projectileLifeTime;
        projectile.damage = projectileDamage;
        projectile.speed = projectileSpeed;
        projectile.target = projectileTarget;
    }

    public override void strongAttack()
    {
        var mousePos = new Vector3(worldPosition.x, player.transform.position.y, worldPosition.z); // Get Mouse Position
        var heading = new Vector3(mousePos.x, this.rb.position.y, mousePos.z) - new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z);//Get direction from pet to mouse.
        heading = heading.normalized; //Normalise direction force.
        var projectile = Instantiate(windProj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);

        projectile.GetComponent<Rigidbody>().velocity = heading;
        projectile.transform.rotation = Quaternion.LookRotation(heading, Vector3.up); //Face current Direction

        projectile.lifeTime = windLifeTime;
        projectile.damage = windDamage;
        projectile.speed = windSpeed;
        projectile.target = projectileTarget;
        projectile.knockBack = knockbackAmt;
        projectile.knockBackDir = heading;


    }

    public override void buffAttack()
    {
        //Buffs the adventurers move speed and then begins timer for buff to last.
        player.moveSpeed += moveSpeedBuff;
        currentMoveSpeedDuration = moveSpeedDuration;
        speedy = true;
    }


}
