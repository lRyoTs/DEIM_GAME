using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance {  get; private set; }

    [Header("Button")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button instructionButton;
    [SerializeField] private Button closeInstructionButton;

    [Header("Panel")]
    [SerializeField] private GameObject instructionPanel;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There is more than 1 instance of MainMenuUI");
        }
        instance = this;

        exitButton.onClick.AddListener(Application.Quit);
        
        newGameButton.onClick.AddListener(() =>
        {
            SoundManager.PlaySound(SoundManager.Sound.Click);
            DataPersistence.Instance.DeletePlayerPrefsInfo();
            DataPersistence.Instance.LoadFromPlayerPrefs();
            Loader.Load(Loader.Scene.Zone1);
        });

        continueGameButton.onClick.AddListener(() =>
        {
            DataPersistence.Instance.LoadFromPlayerPrefs();
            Loader.Load(DataPersistence.Instance.CurrentScene);
        });

        closeInstructionButton.onClick.AddListener(HideInstructionPanel);
        instructionButton.onClick.AddListener(ShowInstructionPanel);

        if (PlayerPrefs.HasKey(DataPersistence.CURRENT_SCENE))
        {
            ShowContinuekButton();
        }
        else
        {
            HideContinueButton();
        }
        HideInstructionPanel();

        SoundManager.CreateSoundManagerGameobject();
    }

    private void Start()
    {
        SoundManager.PlaySong(SoundManager.Sound.Menu);
    }

    public void HideContinueButton()
    {
        continueGameButton.gameObject.SetActive(false);
    }
    public void ShowContinuekButton() {
        continueGameButton.gameObject.SetActive(true);
    }

    public void HideInstructionPanel() {
        instructionPanel.SetActive(false);
    }
    public void ShowInstructionPanel() {
        instructionPanel.SetActive(true);
    }
}
