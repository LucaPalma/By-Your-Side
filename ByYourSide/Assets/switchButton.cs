using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchButton : MonoBehaviour
{
    public bool timedSwitch;
    public bool shootable;
    public float activationTime;
    public bool activated;
    public moveableObject linkedMoveable;
    Transform p;
    public Material def;
    public Material defShootable;
    public Material green;
     

    private void Start()
    {
        if (shootable) this.gameObject.GetComponent<MeshRenderer>().material = defShootable;
        p = FindObjectOfType<Player>().GetComponent<Transform>();     
    }


    void Update()
    {
    if (Input.GetKeyDown(KeyCode.E) && !activated)
        {
        var distanceToPlayer = Mathf.Abs((this.gameObject.transform.position - p.position).magnitude);
        if (distanceToPlayer <= 4)
            {
                activate();
            }
        }        
    }

    void activate()
    {
        if (timedSwitch)
        {
            StartCoroutine("timeOut");
        }
        else
        {
            activated = true;
            this.gameObject.GetComponent<MeshRenderer>().material = green;
            linkedMoveable.reduceRequired();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile" && shootable && !activated)
        {
            if (other.GetComponent<BasicProjectile>() != null)
            {
                if (other.GetComponent<BasicProjectile>().target == "Enemy")
                {
                    activate();
                }
            }
        }
    }

    IEnumerator timeOut()
    {
        activated = true;
        this.gameObject.GetComponent<MeshRenderer>().material = green;
        linkedMoveable.reduceRequired();
        yield return new WaitForSeconds(activationTime);
        activated = false;
        if (shootable) this.gameObject.GetComponent<MeshRenderer>().material = defShootable;
        else this.gameObject.GetComponent<MeshRenderer>().material = def;
        linkedMoveable.increaseRequired();
    }
}
