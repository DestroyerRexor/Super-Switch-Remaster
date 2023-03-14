using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    
    public enum Scene
    {
        Main_Menu_Scene,
        Level_1,
        Level_2,
        Level_3,
        Loading_Scene
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        
        SceneManager.LoadScene(Scene.Loading_Scene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
