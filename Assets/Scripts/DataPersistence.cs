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
    public int CurrentScene { get; set; }

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
    }

    public void SaveInPlayerPrefs() {
        PlayerPrefs.SetInt(PLAYER_CURRENT_EXP,PlayerCurrentExp);
        PlayerPrefs.SetInt(PLAYER_LEVEL, PlayerCurrentLevel);
        PlayerPrefs.SetInt(CURRENT_SCENE, CurrentScene);
        PlayerPrefs.SetFloat(PLAYER_POS_X, PlayerWorldPosition.x);
        PlayerPrefs.SetFloat(PLAYER_POS_Y, PlayerWorldPosition.y);
        PlayerPrefs.SetFloat(PLAYER_POS_Z, PlayerWorldPosition.z);
    }

    public void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }

}
