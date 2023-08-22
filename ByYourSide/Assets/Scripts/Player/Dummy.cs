using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, iDamageable
{
    [Header("Stats")]
    public float health;

    void Update()
    {
        handleHealth();
    }

    public void handleHealth()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void handleDamage(float dmg)
    {
        health -= dmg;
    }

}

