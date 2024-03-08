using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Action loaderCallbackAction;

    //Scene List
    public enum Scene
    {
        MainMenu,
        LoadingScene,
        BattleScene,
        Zone1,
        BossZone
    }

    private static Scene sceneAux;

    public static void Load(Scene scene)
    {
        loaderCallbackAction = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void Load(string scene)
    {
        loaderCallbackAction = () =>
        {
            SceneManager.LoadScene(scene);
        };

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

    public static string GetCurrentScene() {
        return SceneManager.GetActiveScene().name;
    }
}
