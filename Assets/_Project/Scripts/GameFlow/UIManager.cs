using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public static UIManager UI;

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

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
}
