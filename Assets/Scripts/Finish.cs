using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject goalIcon;
    [SerializeField] private string nextScene;

    private void Start()
    {
        if(nextScene == "")
        {
            nextScene = Loader.GetCurrentScene().ToString();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Update DataPersistence to next Level
            DataPersistence.Instance.CurrentScene = nextScene;
            GameManager.Instance.IsWin();
        }
    }
    
}
