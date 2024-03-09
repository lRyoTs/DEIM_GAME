using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("References")]
    private PlayerController _playerController;

    [Header("Stamina")]
    private const int MAX_STAMINA = 100;
    private float lerpTimer;
    private float delayTimer;
    private int currentStamina;

    [Header("UI")]
    [SerializeField] private Image frontStaminaBar;
    [SerializeField] private Image backStaminaBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, MAX_STAMINA);
        UpdateStaminaUI();
    }

    public void UpdateStaminaUI()
    {
        float staminaFraction = currentStamina / MAX_STAMINA;
        float FXP = frontStaminaBar.fillAmount;
        if (FXP < staminaFraction)
        {
            delayTimer += Time.deltaTime;
            backStaminaBar.fillAmount = staminaFraction;
            if (delayTimer > 3)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                frontStaminaBar.fillAmount = Mathf.Lerp(FXP, backStaminaBar.fillAmount, percentComplete);
            }
        }
    }
    public void ConsumeEnergy(float energyConsumed)
    {
        if (currentStamina > 0)
        {
            StartCoroutine("HitCooldown");
            currentStamina -= (int)energyConsumed;
            lerpTimer = 0f;
        }

        if (currentStamina <= 0)
        {
            _playerController.SetPlayerState(PlayerController.PlayerState.Tired);
        }
    }

    public void RestoreHealth(float energyRestored)
    {
        currentStamina += (int)energyRestored;
        lerpTimer = 0;
    }
    public void InitializedPlayerStamina()
    {
        currentStamina = MAX_STAMINA;
    }

    private IEnumerator RestoreEnergyByTime()
    {
        yield return null;
    }
    private IEnumerator RestoreFatigue()
    {
        yield return null;
    }
}
