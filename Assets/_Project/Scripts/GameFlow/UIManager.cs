using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverPanel;
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

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
}
