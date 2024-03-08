using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    [Header("References")]
    private PlayerController playerController;
    private LevelSystem playerLevel;
    [SerializeField] private GameObject spawnPosition;

    [Header("Game Manager Logic")]
    private bool isPaused = false;
    private bool isLose = false;
    private bool isWin = false;

    private void Awake()
    {
        if(Instance != null){
            Debug.Log("There is more than 1 Instance of GameManager");
        }
        Instance = this;

        isPaused = false;
        isLose = false;
        isWin = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsFinish())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                } 
            }
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        PauseUI.instance.Show();
        isPaused = true;
        EventManager.Broadcast(EventManager.EVENT.OnPause);
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        PauseUI.instance.Hide();
        isPaused = false;
        EventManager.Broadcast(EventManager.EVENT.OnResume);
    }

    private void StartGame()
    {
        //Initialize Player
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (PlayerPrefs.HasKey(DataPersistence.PLAYER_POS_X) && PlayerPrefs.HasKey(DataPersistence.PLAYER_POS_Y) && PlayerPrefs.HasKey(DataPersistence.PLAYER_POS_Z))
        {
            spawnPosition.transform.position = DataPersistence.Instance.PlayerWorldPosition;
        }
        playerController.transform.position = spawnPosition.transform.position;
        playerController.ActivateCharacterController(); //Activate Character Controller
        playerLevel = playerController.GetComponent<LevelSystem>();
        playerLevel.InitializedLevelSystem();
        playerController.GetComponent<PlayerStats>().CalculateStats();
        
        Cursor.lockState = CursorLockMode.Locked;
        SoundManager.CreateSoundManagerGameobject();
        SoundManager.PlaySong(SoundManager.Sound.Exploration);
        DataPersistence.Instance.CurrentScene = Loader.GetCurrentScene();
    }

    public bool IsFinish() {
        return isWin || isLose;
    }

    public void IsWin()
    {
        isWin = true;
        WinUI.Instance.Show();
        //Lose Sound

        //Update DataPersistence
        DataPersistence.Instance.PlayerCurrentLevel = playerLevel.Level;
        DataPersistence.Instance.PlayerCurrentExp = (int)playerLevel.CurrentXp;
        //Save in PlayerPrefs
        DataPersistence.Instance.DeletePlayerWorldPos();
        DataPersistence.Instance.SaveInPlayerPrefsProgress();

        Cursor.lockState = CursorLockMode.None;
    }

    public void IsLose()
    {
        isLose = true;
        LoseUI.Instance.Show();

        //Lose Sound
        Cursor.lockState= CursorLockMode.None;
    }
}
