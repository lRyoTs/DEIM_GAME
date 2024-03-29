using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    #region VARIABLES
    public const int MAX_LEVEL = 50;
    public int Level { get; private set; }
    public float CurrentXp {  get; private set; }
    private float requiredXp;

    private float lerpTimer;
    private float delayTimer;

    [Header("UI")]
    [SerializeField] private Image frontXpbar;
    [SerializeField] private Image backXpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Level Multipliers")]
    public int additionMultiplier = 200;
    public int powerMultiplier = 2;
    public int divisionMultiplier = 7;
    #endregion

    // Update is called once per frame
    void Update()
    {
        UpdateXpUI();
        if (CurrentXp > requiredXp) {
            LevelUp();
        }
    }

    public void InitializedLevelSystem()
    {
        Level = DataPersistence.Instance.PlayerCurrentLevel;
        CurrentXp = DataPersistence.Instance.PlayerCurrentExp;
        requiredXp = CalculateRequiredXp();
        frontXpbar.fillAmount = CurrentXp / requiredXp;
        backXpBar.fillAmount = CurrentXp / requiredXp;
        UpdateLevelText();
    }

    public void UpdateXpUI()
    {
        float xpFraction = CurrentXp / requiredXp;
        float FXP = frontXpbar.fillAmount;
        if (FXP < xpFraction) {
            delayTimer += Time.deltaTime;
            backXpBar.fillAmount = xpFraction;
            if (delayTimer > 3) {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / 4;
                frontXpbar.fillAmount = Mathf.Lerp(FXP, backXpBar.fillAmount, percentComplete);
            }
        }
    }

    public void UpdateLevelText()
    {
        levelText.text = Level.ToString();
    }

    public void GainExperienceFlatRate(float xpGained) {
        CurrentXp += xpGained; 
        //Reset Timers
        lerpTimer = 0;
        delayTimer = 0;
    }

    public void LevelUp() {
        if(Level < MAX_LEVEL)
        {
            Level++;
            frontXpbar.fillAmount = 0;
            backXpBar.fillAmount = 0;
            CurrentXp = Mathf.RoundToInt(CurrentXp - requiredXp);
            requiredXp = CalculateRequiredXp();
            UpdateLevelText();
            EventManager.Broadcast(EventManager.EVENT.OnLevelUp);

            //Store in DataPersistence
            DataPersistence.Instance.PlayerCurrentLevel = Level;
            DataPersistence.Instance.PlayerCurrentExp = (int)CurrentXp;
        }
       
    }

    private int CalculateRequiredXp() {
        int solveForRequiredXp = 0;
        for(int levelCycle = 1; levelCycle <= Level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp/4;
    }
}
