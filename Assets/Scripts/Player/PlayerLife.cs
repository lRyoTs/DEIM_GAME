using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private float maxHealth;
    [SerializeField] private float chipSpeed = 2f;
    [SerializeField] public Image frontHealthBar;
    [SerializeField] public Image backHealthBar;


    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
    }

    public void SetMaxHealth(float maxHealth) {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void SetCurrentHealth(float health) {
        this.health = health;
    }

    public float GetCurrentHealth() {
        return health;
    }

    private void UpdateHealthUI()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF > hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount) {
        health += healAmount;
        lerpTimer = 0;
    }
}
