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

    [Header("Air Elemental Bomb Variables")]
    public float bomblifeTime;
    public float bombdamage;
    public float bombknockback;
    public float bombspeed;
    public BombProjectile bombwindProj;
    public float bombwindLifeTime;
    public float bombwindDamage;
    public float bombwindKnockBack;
    public float bombwindSpeed;

    [Header("Air Elemental Ultimate Attack")]
    public float projectileNum;
    public float waveNum;


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

    public override void fourAttack()
    {
        var mousePos = new Vector3(worldPosition.x, player.transform.position.y, worldPosition.z); // Get Mouse Position
        var heading = new Vector3(mousePos.x, this.rb.position.y, mousePos.z) - new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z);//Get direction from pet to mouse.
        heading = heading.normalized; //Normalise direction force.
        var projectile = Instantiate(bombwindProj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = heading;

        projectile.lifeTime = bomblifeTime;
        projectile.damage = bombdamage;
        projectile.speed = bombspeed;
        projectile.target = projectileTarget;
        projectile.knockback = bombknockback;

        projectile.windLifeTime = bombwindLifeTime;
        projectile.windDamage = bombwindDamage;
        projectile.windKnockBack = bombwindKnockBack;
        projectile.windSpeed = bombwindSpeed;
        projectile.windTarget = projectileTarget;
        

    }

    public override void fiveAttack()
    {
        //
    }

    public override void ultAttack()
    {
        StartCoroutine("UltRoutine");
    }

    public IEnumerator UltWave()
    {         
        float radius = 5f;
        float angleStep = 360f / projectileNum;
        float angle = 0f;

        for (int i = 0; i <= projectileNum; i++)
        {

            float directionX = Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float directionZ = Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector3 projectileVector = new Vector3(directionX, 0, directionZ);
            Vector3 projectileMoveDirection = (projectileVector).normalized * projectileSpeed;
            var projectile = Instantiate(bombwindProj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x, 0, projectileMoveDirection.z);

            projectile.lifeTime = bomblifeTime;
            projectile.damage = bombdamage;
            projectile.speed = bombspeed;
            projectile.target = projectileTarget;
            projectile.knockback = bombknockback;

            projectile.windLifeTime = bombwindLifeTime;
            projectile.windDamage = bombwindDamage;
            projectile.windKnockBack = bombwindKnockBack;
            projectile.windSpeed = bombwindSpeed;
            projectile.windTarget = projectileTarget;

            angle += angleStep;
        }
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator UltRoutine()
    {
        for (int i = 0; i <= waveNum; i++)
        {
            StartCoroutine("UltWave");
            yield return new WaitForSeconds(0.7f);
        }
    }
}
