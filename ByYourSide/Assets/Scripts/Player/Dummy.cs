using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dummy : MonoBehaviour, iDamageable
{
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public bool boss = false;

    [Header("Sounds")]
    [SerializeField] private string deathName = "EnemyDeath";
	private AudioSource deathSound;

    [Header("Mats")]
    public Material red;
    public Material white;

    private void Awake()
	{
        deathSound = GameObject.Find(deathName).GetComponent<AudioSource>();
	}

    void Update()
    {
        handleHealth();
    }

    public void handleHealth()
    {
        if (health <= 0)
        {
            deathSound.Play();
            if (boss)
            {
                SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
            }
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            //this.gameObject.transform.position = new Vector3(-390, 20.35167f, -5.01f);
            //foreach( Transform child in transform )
            //{
            //    child.gameObject.SetActive( false );
            //}
            //this.gameObject.GetComponent<BaseEnemy>().enabled = false;
            //this.gameObject.GetCompsonent<BoxCollider>().enabled = false;
            //health = maxHealth;
        }
        
    }

    public void handleDamage(float dmg)
    {
        if (!boss) StartCoroutine("FlashRed");
        health -= dmg;
    }

    public void resetHealth()
    {
        health = maxHealth;
    }

    IEnumerator FlashRed()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer R in renderers)
        {
            R.material = red;
        }
        yield return new WaitForSeconds(0.2f);
        foreach (MeshRenderer R in renderers)
        {
            R.material = white;
        }

    }

}

