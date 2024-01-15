using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    // Variable que guarda una funci�n sin inputs ni output
    private static Action loaderCallbackAction;

    // Una clase static tiene todas sus variables y funciones tambi�n static

    // Lista de nuestras escenas
    public enum Scene
    {
        LoadingScene
    }

    private static Scene sceneAux;

    public static void Load(Scene scene)
    {
        // Asignas en loaderCallbackAction una funci�n que no recibe par�metros y ejecuta la l�nea 25
        loaderCallbackAction = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };


        // Llamamos a la escena de carga
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        if (loaderCallbackAction != null)
        {
            loaderCallbackAction();
            loaderCallbackAction = null;
        }
    }


    // () => { cuerpo funci�n }
    /*
     * private void NombreAux(){
     * cuerpo funci�n
     * }
     */
}
