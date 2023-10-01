using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public float healAmount = 12f;
    public bool alreadyHealed;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("triggered check");
            var p = collision.GetComponent<Player>();

            if (!alreadyHealed)
            {
                p.currentCheckpoint = new Vector3(transform.position.x, 1.14f, transform.position.z);
                p.currentHealth = p.currentHealth + healAmount;
                if (p.currentHealth > p.maxHealth) p.currentHealth = p.maxHealth;
                p.healthBar.SetHealth(p.currentHealth);
                alreadyHealed = true;
            }

        }
    }




}
