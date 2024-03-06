using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script that Manages PlayerLife
public class PlayerLife : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private float maxHealth;
    [SerializeField] private float chipSpeed = 2f;
    [SerializeField] public Image frontHealthBar;
    [SerializeField] public Image backHealthBar;

    public bool isDead { get; private set;}
    private bool canTakeDamage = true;
    [SerializeField] private float hitCooldown = 1f;

    private void Awake()
    {
        isDead = false;
        canTakeDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
            health = Mathf.Clamp(health, 0, maxHealth);
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
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth; //Health ratio
        
        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
        
        Debug.Log($"Health:{health} MaxHealth:{maxHealth}");
    }

    public void TakeDamage(float damage) {
        if (canTakeDamage)
        {
            StartCoroutine("HitCooldown");
            health -= damage;
            lerpTimer = 0f;
        }

        if(health < 0)
        {
            isDead = true;
        }
    }

    public void RestoreHealth(float healAmount) {
        health += healAmount;
        lerpTimer = 0;
    }

    public void RestoreToMaxHealth()
    {
        health = maxHealth;
    }

    private IEnumerator HitCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(hitCooldown);
        canTakeDamage = true;
    }
}
