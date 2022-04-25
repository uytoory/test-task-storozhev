using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthFill = null;

    public void SetUp(float maxHealth)
    {
        healthFill.maxValue = maxHealth;
        healthFill.value = maxHealth;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetHealth(float health)
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        healthFill.value = health;
    }
}
