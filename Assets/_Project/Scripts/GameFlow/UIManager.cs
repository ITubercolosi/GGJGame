using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverPanelFuel, WinPanel, GameOverDestroyed;
    public static UIManager UI;
    public TMPro.TextMeshProUGUI ScoreText;


    private void Start()
    {
        if(UI == null)
        {
            UI = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    
    public void SetScoreText(int score)
    {
        ScoreText.text = string.Format("Score:{0}", score);
    }

    public void ShowGameOverPanelDestroyed()
    {
        GameOverDestroyed.SetActive(true);
    }

    public void ShowGameOverPanelOutOfFuel()
    {
        GameOverPanelFuel.SetActive(true);
    }

    public void ShowWinPanel()
    {
        WinPanel.SetActive(true);
    }

    public void RestartGame()
    {
        GameSceneManager.instance.ReloadGame();
    }

    public void PlayNextLevel(int levelIndex)
    {
        GameSceneManager.instance.PlayNextLevel(levelIndex);
    }
}
