using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, iDamageable
{
    [Header("Stats")]
    public float health;

    [Header("Sounds")]
    [SerializeField] private string deathName;
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
            Destroy(this.gameObject);
        }
    }

    public void handleDamage(float dmg)
    {
        health -= dmg;
    }

}

