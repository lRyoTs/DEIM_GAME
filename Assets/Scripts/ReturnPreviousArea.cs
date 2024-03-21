using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReturnPreviousArea : MonoBehaviour
{
    [Header("References")]
    private PlayerControls input;

    [SerializeField] private TextMeshProUGUI areaToReturnText;
    [SerializeField] private Loader.Scene areaToLoad;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private string message;
    [SerializeField] private ParticleSystem areaParticle;

    
    private void Start()
    {
        input = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
        areaToReturnText.text = areaToLoad.ToString();   
    }

    private void OnTriggerEnter(Collider other)
    {
        areaParticle.Play();
        messageText.text = message;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")&& input.Interact)
        {
            input.Interact = false;
            DataPersistence.Instance.DeletePlayerWorldPos();
            Loader.Load(areaToLoad);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        areaParticle.Play();
        messageText.text = "";
    }
}
