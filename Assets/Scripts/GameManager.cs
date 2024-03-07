using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [SerializeField] private GameObject spawnPosition;
    private PlayerController player;
    private bool isPaused = false;
    private bool isFinish = false;

    private void Awake()
    {
        if(Instance != null){
            Debug.Log("There is more than 1 Instance of GameManager");
        }
        Instance = this;

        isPaused = false;
        isFinish = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinish)
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        Debug.Log($"Previuos spawm position {spawnPosition.transform.position}/// Player position:{player.transform.position}");
        if (PlayerPrefs.HasKey(DataPersistence.PLAYER_POS_X) && PlayerPrefs.HasKey(DataPersistence.PLAYER_POS_Y) && PlayerPrefs.HasKey(DataPersistence.PLAYER_POS_Z))
        {
            spawnPosition.transform.position = DataPersistence.Instance.PlayerWorldPosition;
            Debug.Log($"New spawn position {spawnPosition.transform.position}");
        }

        Debug.Log($" Current position {player.transform.position}");
        player.transform.position = spawnPosition.transform.position;
        Debug.Log($" New player position {player.transform.position}");
        player.ActivateCharacterController(); //Activate Character Controller
        player.GetComponent<LevelSystem>().InitializedLevelSystem();
        player.GetComponent<PlayerStats>().CalculateStats();
        SoundManager.CreateSoundManagerGameobject();
        SoundManager.PlaySong(SoundManager.Sound.Exploration);
        DataPersistence.Instance.CurrentScene = Loader.GetCurrentScene();
    }
}
