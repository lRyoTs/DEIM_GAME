using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static Action loaderCallbackAction;

    //Scene List
    public enum Scene
    {
        LoadingScene,
        BattleScene,
        Zone01
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
}
