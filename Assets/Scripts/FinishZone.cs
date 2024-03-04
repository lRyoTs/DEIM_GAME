using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FinishZone : MonoBehaviour
{   
    [SerializeField] private GameObject finishPanel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            finishPanel.SetActive(true);
        }
    }
}
