using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("References")]
    private PlayerController player;
    private PlayerControls playerInput;
    private LevelSystem playerLevel;

    [Header("CheckPoint UI")]
    [SerializeField] private ParticleSystem checkpointParticles;
    [SerializeField] private GameObject interactInput;
    // Start is called before the first frame update
    void Start()
    {
        interactInput.SetActive(false);
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerInput = player.GetComponent<PlayerControls>();
        playerLevel = player.GetComponent<LevelSystem>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerLife>().RestoreToMaxHealth();
            DataPersistence.Instance.PlayerWorldPosition = player.transform.position;
            DataPersistence.Instance.PlayerCurrentLevel = playerLevel.Level;
            DataPersistence.Instance.PlayerCurrentExp = (int)playerLevel.CurrentXp;

            checkpointParticles.Play();
            interactInput.SetActive(true);
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && playerInput.Interact)
        {
            playerInput.Interact = false;
            DataPersistence.Instance.SaveInPlayerPrefs();
            SoundManager.PlaySound(SoundManager.Sound.Save);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            checkpointParticles.Stop();
            interactInput.SetActive(false);
        }
    }
}
