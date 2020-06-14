using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class Loader
{
    public enum Scene
    {
        level1, level2, level3, level4, level5, level6, level7, Thank, Loading
    }

    public static Scene nextScene;

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {
        //set the loader callback action to load the target scene
        onLoaderCallback = () =>
        {
            {
                SceneManager.LoadScene(scene.ToString());
            }
        };

        //load the loading scene
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        //triggered after the first update which lets the screen refresh
        //execute the loader callback action which will load the target scene
        if(onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
