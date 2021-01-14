using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Slider slider;

    public void SetCooldown(float cooldown)
    {
        slider.value = cooldown;
    }

    public void SetMaxCooldown(float cooldown)
    {
        slider.maxValue = cooldown;
        slider.value = cooldown;
    }
}
