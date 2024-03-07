using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence Instance { get; private set; }

    #region PlayerPrefsKeys
    public const string PLAYER_LEVEL = "PLAYER_LEVEL";
    public const string PLAYER_CURRENT_EXP = "PLAYER_EXP";
    public const string CURRENT_SCENE = "CURRENT_SCENE";
    public const string PLAYER_POS_X = "PLAYER_POS_X";
    public const string PLAYER_POS_Y = "PLAYER_POS_Y";
    public const string PLAYER_POS_Z = "PLAYER_POS_Z";

    #endregion

    public Vector3 PlayerWorldPosition { get; set; } //Store Player world position
    public int PlayerCurrentLevel { get; set; }
    public int PlayerCurrentExp { get; set; }
    public string CurrentScene { get; set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }

        if (PlayerPrefs.HasKey(PLAYER_LEVEL))
        {
            GetPlayerPrefsCurrentScene();
            GetPlayerPrefsLevel();
            GetPlayerPrefsExp();
            GetPlayersPrefsPlayerPosition();
        }
    }
    /// <summary>
    /// Save all game information in player prefs
    /// </summary>
    public void SaveInPlayerPrefs() {
        PlayerPrefs.SetInt(PLAYER_CURRENT_EXP,PlayerCurrentExp);
        PlayerPrefs.SetInt(PLAYER_LEVEL, PlayerCurrentLevel);
        PlayerPrefs.SetString(CURRENT_SCENE, CurrentScene);
        PlayerPrefs.SetFloat(PLAYER_POS_X, PlayerWorldPosition.x);
        PlayerPrefs.SetFloat(PLAYER_POS_Y, PlayerWorldPosition.y);
        PlayerPrefs.SetFloat(PLAYER_POS_Z, PlayerWorldPosition.z);
    }

    /// <summary>
    /// Delete all player prefs unrelated to sound or settings
    /// </summary>
    public void DeletePlayerPrefsInfo() {
        PlayerPrefs.DeleteKey(PLAYER_CURRENT_EXP);
        PlayerPrefs.DeleteKey(PLAYER_LEVEL);
        PlayerPrefs.DeleteKey(CURRENT_SCENE);
        PlayerPrefs.DeleteKey(PLAYER_POS_X);
        PlayerPrefs.DeleteKey(PLAYER_POS_Y);
        PlayerPrefs.DeleteKey(PLAYER_POS_Z);

    }

    public void GetPlayersPrefsPlayerPosition() {
        PlayerWorldPosition = new Vector3(PlayerPrefs.GetFloat(PLAYER_POS_X, 0), PlayerPrefs.GetFloat(PLAYER_POS_Y, 0), PlayerPrefs.GetFloat(PLAYER_POS_Z, 0));
    }

    public void GetPlayerPrefsLevel()
    {
        PlayerCurrentLevel = PlayerPrefs.GetInt(PLAYER_LEVEL, 1);
    }

    public void GetPlayerPrefsExp() {
        PlayerCurrentExp = PlayerPrefs.GetInt(PLAYER_CURRENT_EXP, 0);
    }

    public void GetPlayerPrefsCurrentScene() {
        CurrentScene = PlayerPrefs.GetString(CURRENT_SCENE, "Zone1");
    }

    /// <summary>
    /// Load Game Information
    /// </summary>
    public void LoadFromPlayerPrefs()
    {
        GetPlayerPrefsExp();
        GetPlayerPrefsLevel();
        GetPlayersPrefsPlayerPosition();
        GetPlayerPrefsCurrentScene();
    }

    /// <summary>
    /// Delete PlayerWorldPos player prefs once players wins
    /// </summary>
    public void DeletePlayerWorldPos()
    {
        PlayerPrefs.DeleteKey(PLAYER_POS_X);
        PlayerPrefs.DeleteKey(PLAYER_POS_Y);
        PlayerPrefs.DeleteKey(PLAYER_POS_Z);
    }

    /// <summary>
    /// Saves information about level and next scene
    /// </summary>
    public void SaveInPlayerPrefsProgress()
    {
        PlayerPrefs.SetInt(PLAYER_CURRENT_EXP, PlayerCurrentExp);
        PlayerPrefs.SetInt(PLAYER_LEVEL, PlayerCurrentLevel);
        PlayerPrefs.SetString(CURRENT_SCENE, CurrentScene);
    }
}
