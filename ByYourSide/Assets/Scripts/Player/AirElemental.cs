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

    [Header("Air Elemental Lightning Variables")]
    public BasicProjectile bolt;
    public float boltLifeTime;
    public float boltDamage;
    public float boltSpeed;
    private float boltKnockback;
    [SerializeField] private float boltNum;
    [SerializeField] private float spreadRadius;

    [Header("Air Elemental Ultimate Attack")]
    public float projectileNum;
    public float waveNum;

    [Header("Sounds")]
    [SerializeField] private string shootName = "PetShoot";
	private AudioSource shootSound;
    [SerializeField] private string tornadoName = "PetTornado";
	private AudioSource tornadoSound;
    [SerializeField] private string zoomName = "PetZoom";
	private AudioSource zoomSound;
    [SerializeField] private string shotgunName = "PetShotgun";
	private AudioSource shotgunSound;
    [SerializeField] private string ultimateName = "PlayerUltimate";
	private AudioSource ultimateSound;


    private void Awake()
	{
        shootSound = GameObject.Find(shootName).GetComponent<AudioSource>();
        tornadoSound = GameObject.Find(tornadoName).GetComponent<AudioSource>();
        zoomSound = GameObject.Find(zoomName).GetComponent<AudioSource>();
        shotgunSound = GameObject.Find(shotgunName).GetComponent<AudioSource>();
        ultimateSound = GameObject.Find(ultimateName).GetComponent<AudioSource>();
	}

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
        shootSound.Play();
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
        tornadoSound.Play();
    }

    public override void buffAttack()
    {
        //Buffs the adventurers move speed and then begins timer for buff to last.
        player.moveSpeed += moveSpeedBuff;
        currentMoveSpeedDuration = moveSpeedDuration;
        speedy = true;
        zoomSound.Play();
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
        
        shootSound.Play();
    }

    public override void fiveAttack()
    {
        float angleStep = 180f / boltNum;
        float angle = 0f;
        var mousePos = new Vector3(worldPosition.x, player.transform.position.y, worldPosition.z); // Get Mouse Position
        var heading = new Vector3(mousePos.x, this.rb.position.y, mousePos.z) - new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z);//Get direction from pet to mouse.

        for (int i = 0; i <= boltNum; i++)
        {

            float dirX = heading.x + Mathf.Sin((angle * Mathf.PI) / 180) * spreadRadius;
            float dirZ = heading.z + Mathf.Cos((angle * Mathf.PI) / 180) * spreadRadius;

            var projectile = Instantiate(bolt, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = new Vector3(dirX, 0, dirZ).normalized * boltSpeed;

            projectile.lifeTime = boltLifeTime;
            projectile.damage = boltDamage;
            projectile.speed = boltSpeed;
            projectile.knockback = boltKnockback;
            projectile.target = projectileTarget;
            projectile.transform.rotation = Quaternion.LookRotation(projectile.GetComponent<Rigidbody>().velocity.normalized, Vector3.up); //Face current Direction

            angle += angleStep;
        }
        shootSound.Play();
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
        ultimateSound.Play();
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
