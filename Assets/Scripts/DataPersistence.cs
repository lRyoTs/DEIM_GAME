using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence Instance { get; private set; }

    public Vector3 PlayerWorldPosition { get; set; } //Store Player world position
    public int PlayerCurrentHealt { get; set; }
    public int currentScene { get; set; }
    public int battleExp { get; private set; }

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
}
