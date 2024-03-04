using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public static PauseUI instance { get; private set; }

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There is more than 1 instance of PauseUI");
        }
        instance = this;

        mainMenuButton.onClick.AddListener(() => {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            Time.timeScale = 1f;
            SoundManager.PlaySong(SoundManager.Sound.Menu);
            Loader.Load(Loader.Scene.MainMenu);
        });

        resumeButton.onClick.AddListener(() => {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            GameManager.Instance.ResumeGame();
        });

        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
