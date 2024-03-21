using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private string message;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messageText.text = message;
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messageText.text = "";
        }
        
    }
}
