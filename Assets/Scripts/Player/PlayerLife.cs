using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Script that Manages PlayerLife
public class PlayerLife : MonoBehaviour
{
    [Header("References")]
    private PlayerController playerController;

    [Header("HP")]
    private float health;
    private float lerpTimer;
    private float maxHealth;
    [SerializeField] private float chipSpeed = 2f;
    [SerializeField] public Image frontHealthBar;
    [SerializeField] public Image backHealthBar;
    [SerializeField] public TextMeshProUGUI playerHpText;

    public bool isDead { get; private set;}
    private bool canTakeDamage = true;
    [SerializeField] private float hitCooldown = 1f;

    private void Awake()
    {
        isDead = false;
        canTakeDamage = true;
        playerController = GetComponent<PlayerController>();
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
        float hFraction = health / maxHealth; //Health ratio gives a number between 0 and 1 
        
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
        playerHpText.text = $"{health} / {maxHealth}";
    }

    public void TakeDamage(float damage) {
        if (canTakeDamage)
        {
            StartCoroutine("HitCooldown");
            health -= damage;
            lerpTimer = 0f;
        }

        if(health <= 0)
        {
            isDead = true;
            playerController.SetPlayerState(PlayerController.PlayerState.Dead);
            GameManager.Instance.IsLose(); //Temporal placement
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
