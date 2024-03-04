using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance { get; private set; }
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than 1 instance of WinUI");
        }
        Instance = this;

        mainMenuButton.onClick.AddListener(() =>
        {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            Loader.Load(Loader.Scene.MainMenu);
        });

        gameObject.SetActive(false);
    }

}
