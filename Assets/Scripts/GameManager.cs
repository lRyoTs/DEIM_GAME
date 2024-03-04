using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [SerializeField] private GameObject spawnPosition;
    private bool isPaused = false;

    private void Awake()
    {
        if(Instance != null){
            Debug.Log("There is more than 1 Instance of GameManager");
        }
        Instance = this;
        
        if(DataPersistence.Instance.PlayerWorldPosition != Vector3.zero)
        {
            spawnPosition.transform.position = DataPersistence.Instance.PlayerWorldPosition;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        SoundManager.CreateSoundManagerGameobject();
    }
}
