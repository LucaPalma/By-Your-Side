using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, iDamageable, iKnockBackable
{
    //References
    public Rigidbody rb;
    public Material normalColour;
    public Material dodgeColour;
    public GameObject healthBarObj;
    cooldownTimer healthBar;
    Pet pet;
    public Vector3 currentCheckpoint;


    //Input Bools
    bool wantToDodge;
    bool wantToCombo;
    bool wantToInteract;

    [Header("Movement Stats")]
    public float moveSpeed;
    float intentionX;
    float intentionY;


    [Header("Stats")]
    public float maxHealth;
    public float currentHealth;
    public string currentPet = "Air";

    public float damageInvulnTimerMax;
    public float damageInvulnTimer; //Invuln from taking damage.

    [Header("Air Combo Stats")]
    public KnockBackProjectile windProj;
    public float windLifeTime;
    public float windDamage;
    public float knockbackAmt;
    public float windSpeed;

    [Header("Cooldowns")]
    public float comboMaxCD;
    float comboCurrentCD = 0;
    public GameObject comboGuiObj;
    private cooldownTimer comboGui;
    public float dodgeMaxCD;
    public float dodgeCurrentCD;
    public GameObject dodgeGuiObj;
    private cooldownTimer dodgeGui;

    [Header("Dodge Variables")]
    public float dodgeSpeed;
    public float dodgeDuration;
    float currentdodgeDuration;
    public bool dashInvuln;//Active while player is buffed.
 


    private void Start()
    {
        currentCheckpoint = this.transform.position; //Update first checkpoint to be spawn location
        rb = GetComponent<Rigidbody>();
        pet = FindObjectOfType<Pet>();

        //Find Gui
        comboGui = comboGuiObj.GetComponent<cooldownTimer>();
        dodgeGui = dodgeGuiObj.GetComponent<cooldownTimer>();
        healthBar = healthBarObj.GetComponent<cooldownTimer>();

        //Set Gui Element Values
        comboGui.SetMaxHealth(comboMaxCD);
        comboGui.SetHealth(0);
        dodgeGui.SetMaxHealth(dodgeMaxCD);
        dodgeGui.SetHealth(0);

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);

    }

    void Update()
    {
        handleInput();
        handleAbilities();
        handleRender();

        //Dodge Updating
        currentdodgeDuration -= Time.deltaTime;
        if (currentdodgeDuration <= 0 && dashInvuln)
        {
            dashInvuln = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            this.GetComponent<MeshRenderer>().material = normalColour;
        }

        //Invuln updating
        damageInvulnTimer -= Time.deltaTime;
        if (damageInvulnTimer <= 0) damageInvulnTimer = 0;

        //Ensure player cannot fly up vertically.
        if (rb.velocity.y > 0) { rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z); }

        //Set player back to checkpoint if they fall off.
        if (this.transform.position.y < -10)
        {
            Debug.Log("called respawn");
            rb.position = currentCheckpoint;
        }

    }
    private void FixedUpdate()
    {
        if (damageInvulnTimer < damageInvulnTimerMax/3) //Only move if havent recently taken damage, allows knockback to take effect for a bit.
        {
            handleMovement();
        }
    }
    public void handleInput()
    {
        intentionX = Input.GetAxis("Horizontal"); //A and D scale from -1 to 1
        intentionY = Input.GetAxis("Vertical"); //W and S scale from -1 to 1

        if (Input.GetKeyDown(KeyCode.Space))
        {
            wantToDodge = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            wantToCombo = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            wantToInteract = true;
        }
    }

    public void handleAbilities()
    {
        if (wantToDodge && dodgeCurrentCD <= 0)
        {
            dodge();
            wantToDodge = false;
            dodgeCurrentCD = dodgeMaxCD;
        }

        if (wantToCombo && comboCurrentCD <= 0)
        {
            combo();
            wantToCombo = false;
            comboCurrentCD = comboMaxCD;
        }

        //Reset Triggers
        wantToDodge = false;
        wantToCombo = false;
        wantToInteract = false;

        //Cooldown Updates
        comboCurrentCD -= Time.deltaTime;
        if (comboCurrentCD <= 0) comboGui.SetHealth(0);
        else comboGui.SetHealth(comboCurrentCD);

        dodgeCurrentCD -= Time.deltaTime;
        if (dodgeCurrentCD <= 0) dodgeGui.SetHealth(0);
        else dodgeGui.SetHealth(dodgeCurrentCD);
    }

    public void dodge()
    {
        this.rb.velocity = this.rb.velocity.normalized * dodgeSpeed;

        currentdodgeDuration = dodgeDuration;
        dashInvuln = true;
        //Allow player to dodge over gaps
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }

    public void combo()
    {
        switch (currentPet)
        {
            case "Air":
                var projectile = Instantiate(windProj, new Vector3(this.rb.position.x, this.rb.position.y, this.rb.position.z), Quaternion.identity);
                projectile.lifeTime = windLifeTime;
                projectile.damage = windDamage;
                projectile.speed = windSpeed;
                projectile.target = "Enemy";
                projectile.knockBack = knockbackAmt;
                break;

            case "Fire":

                break;
        }

    }

    public void interact()
    {


    }

    public void handleRender()
    {
        if (dashInvuln || damageInvulnTimer > 0)
        {
            this.GetComponent<MeshRenderer>().material = dodgeColour;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = normalColour;
        }
    }

    public void handleMovement()
    {
        //Update players velocity based on intention if not dodging.
        if (currentdodgeDuration <= 0)
        {
            var newVel = new Vector3((moveSpeed) * intentionX, rb.velocity.y, (moveSpeed) * intentionY);
            rb.velocity = newVel;
            pet.rb.velocity = newVel;
        }
        else
        {
            pet.rb.velocity = this.rb.velocity;
        }

    }

    public void handleDamage(float dmg)
    {

        if (!dashInvuln && damageInvulnTimer <= 0)
        {
            currentHealth -= dmg;
            damageInvulnTimer = damageInvulnTimerMax;
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            die();
        }
    }

    public void handleKnockBack(float power, Vector3 source)
    {
        if (!dashInvuln && damageInvulnTimer <= 0)
        {
            this.rb.velocity = (rb.position - source).normalized * power;
            pet.rb.velocity = (rb.position - source).normalized * power;
        }
    }


    public void die()
    {
        //death logic.
    }
}
