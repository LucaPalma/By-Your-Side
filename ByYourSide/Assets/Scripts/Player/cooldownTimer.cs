using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cooldownTimer : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float MaxCooldown)
    {
        slider.maxValue = MaxCooldown;
        slider.value = MaxCooldown;
    }

    public void SetHealth(float Cooldown)
    {
        slider.value = Cooldown;
    }


}
