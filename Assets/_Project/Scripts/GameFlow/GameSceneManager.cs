using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    public static GameSceneManager instance;
    private void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void LoadGameScene()
    {
        PlayNextLevel(1);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void PlayNextLevel(int sceneIndex)
    {
        if(sceneIndex == -1)
        {
            SceneManager.LoadScene("Scene_MainMenu");
        }
        else
        {
            SceneManager.LoadScene( string.Format("Scene_Level_{0}", sceneIndex));
        }
    }
}
