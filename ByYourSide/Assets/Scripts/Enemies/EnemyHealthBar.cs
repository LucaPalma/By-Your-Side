using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void SetMaxHealth2(float MaxCooldown)
    {
        slider.maxValue = MaxCooldown;
        slider.value = MaxCooldown;
    }

    public void SetHealth2(float Cooldown)
    {
        slider.value = Cooldown;
    }

    public void Update()
    {
        //transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }


}
