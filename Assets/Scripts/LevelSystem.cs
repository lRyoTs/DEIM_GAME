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
    private float currentXp;
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


    // Start is called before the first frame update
    void Start()
    {
        Level = DataPersistence.Instance.PlayerCurrentLevel;
        currentXp = DataPersistence.Instance.PlayerCurrentExp;
        requiredXp = CalculateRequiredXp();
        frontXpbar.fillAmount = currentXp / requiredXp;
        backXpBar.fillAmount = currentXp / requiredXp;
        UpdateLevelText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateXpUI();
        if (currentXp > requiredXp) {
            LevelUp();
        }
    }

    public void UpdateXpUI()
    {
        float xpFraction = currentXp / requiredXp;
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
        currentXp += xpGained; 
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
            currentXp = Mathf.RoundToInt(currentXp - requiredXp);
            requiredXp = CalculateRequiredXp();
            UpdateLevelText();
            EventManager.Broadcast(EventManager.EVENT.OnLevelUp);

            //Store in DataPersistence
            DataPersistence.Instance.PlayerCurrentLevel = Level;
            DataPersistence.Instance.PlayerCurrentExp = (int)currentXp;
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
