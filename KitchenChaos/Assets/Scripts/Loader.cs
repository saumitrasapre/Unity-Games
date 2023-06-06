using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Scene targetScene;

    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene    
    }

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
