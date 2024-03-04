using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcesingManager : MonoBehaviour
{
    public static PostProcesingManager instance { get; private set; }
    private Volume volumen;
    private Vignette vignette;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There is more than 1 instance of PostProcesingManager");
        }
        instance = this;

        volumen = GetComponent<Volume>();
    }

    // Start is called before the first frame update
    void Start()
    {
        volumen.profile.TryGet(out vignette);
        VignetteOff();
    }

    private void OnDisable()
    {
    }

    public void VignetteOn()
    {
        vignette.active = true;
        Invoke("VignetteOff", 3f);
    }

    public void VignetteOff() {
        vignette.active = false;
    }
}
