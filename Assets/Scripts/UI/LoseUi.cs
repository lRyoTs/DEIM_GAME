using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    public static LoseUI Instance { get; private set; }
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retyButton;
    [SerializeField] private TextMeshProUGUI tipsText;
    [SerializeField] private List<string> tipsList;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of WinUI");
        }
        Instance = this;

        mainMenuButton.onClick.AddListener(() =>
        {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            Loader.Load(Loader.Scene.MainMenu);
        });

        retyButton.onClick.AddListener(() => {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            DataPersistence.Instance.LoadFromPlayerPrefs();
            Loader.Load(DataPersistence.Instance.CurrentScene);
        });
        WriteRandomTip();
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);   
    }

    private void WriteRandomTip()
    {
        int getIndex = Random.Range(0,tipsList.Count -1);
        tipsText.text = $"TIP: {tipsList[getIndex]}";
    }
}
