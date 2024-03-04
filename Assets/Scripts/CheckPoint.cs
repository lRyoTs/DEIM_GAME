using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private ParticleSystem checkpointParticles;
    [SerializeField] private GameObject interactInput;
    // Start is called before the first frame update
    void Start()
    {
        interactInput.SetActive(false);
        player = GameObject.FindWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerLife>().RestoreToMaxHealth();
            DataPersistence.Instance.PlayerWorldPosition = player.transform.position;
            checkpointParticles.Play();
            interactInput.SetActive(true);
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && player.GetComponent<PlayerControls>().Interact)
        {
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
