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
    private const float DEFAULT_RECOVERY_UNIT = 5f;
    private const float TIRED_RECOVERY_UNIT = 10f; //Switch to this value if Player is Tired
    private const float MAX_STAMINA = 50;
    public float staminaRecoveyUnit = 5;
    private const float staminaRecoveryTime = DEFAULT_RECOVERY_UNIT;
    private float lerpTimer;
    [SerializeField] private float chipSpeed;
    private float currentStamina;

    [Header("UI")]
    [SerializeField] private Image frontStaminaBar;
    [SerializeField] private Image backStaminaBar;
    [SerializeField] private TextMeshProUGUI fatigueText;
    [SerializeField] private TextMeshProUGUI staminaText;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        HideFatigueText();
        EventManager.AddHandler(EventManager.EVENT.OnLevelUp,SetMaxStamina);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializedPlayerStamina();
        StartCoroutine("RestoreEnergyByTime");
    }

    // Update is called once per frame
    void Update()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, MAX_STAMINA);
        UpdateStaminaUI();
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(EventManager.EVENT.OnLevelUp, SetMaxStamina);    
    }

    public void UpdateStaminaUI()
    {
        float fillF = frontStaminaBar.fillAmount;
        float fillB = backStaminaBar.fillAmount;
        float StaminaFraction = currentStamina / MAX_STAMINA; //Stamina ratio gives a number between 0 and 1 

        if (fillB > StaminaFraction)
        {
            frontStaminaBar.fillAmount = StaminaFraction;
            backStaminaBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backStaminaBar.fillAmount = Mathf.Lerp(fillB, StaminaFraction, percentComplete);
        }

        if (fillF < StaminaFraction)
        {
            backStaminaBar.fillAmount = StaminaFraction;
            backStaminaBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontStaminaBar.fillAmount = Mathf.Lerp(fillF, backStaminaBar.fillAmount, percentComplete);
        }
        staminaText.text = $"{currentStamina} / {MAX_STAMINA}";
    }
    public void ConsumeEnergy(float energyConsumed)
    {
        if (currentStamina > 0)
        {
            currentStamina -= energyConsumed;
            lerpTimer = 0f;
        }

        if (currentStamina <= 0)
        {
            StartCoroutine("RestoreFatigue"); //Fstigue State player cant Shoot or Dash
        }
    }

    public void RestoreEnergy(float energyRestored)
    {
        currentStamina += energyRestored;
        lerpTimer = 0;
    }

    public void InitializedPlayerStamina()
    {
        currentStamina = MAX_STAMINA;
    }

    public void SetMaxStamina()
    {
        currentStamina = MAX_STAMINA;
    }

    /// <summary>
    /// COroutine to restore energy by time
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestoreEnergyByTime()
    {
        
        while (!GameManager.Instance.IsFinish())
        {
            yield return new WaitWhile(() => currentStamina == MAX_STAMINA);
            yield return new WaitForSeconds(staminaRecoveryTime);
            RestoreEnergy(staminaRecoveyUnit);
        }
    }

    /// <summary>
    /// Coroutine that manages player recovery from Fatigue state
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestoreFatigue()
    {
        _playerController.SetPlayerState(PlayerController.PlayerState.Tired);
        staminaRecoveyUnit = TIRED_RECOVERY_UNIT;
        ShowFatigueText();

        yield return new WaitWhile(() => currentStamina < MAX_STAMINA);

        staminaRecoveyUnit = DEFAULT_RECOVERY_UNIT;
        HideFatigueText();
        _playerController.SetPlayerState(PlayerController.PlayerState.Normal);
    }

    private void ShowFatigueText() {
        fatigueText.gameObject.SetActive(true);
    }

    private void HideFatigueText()
    {
        fatigueText.gameObject.SetActive(false);
    }
}

