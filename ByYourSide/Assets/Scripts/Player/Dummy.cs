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
        }
    }

    public void handleDamage(float dmg)
    {
        health -= dmg;
    }

    public void resetHealth()
    {
        health = maxHealth;
    }

}

