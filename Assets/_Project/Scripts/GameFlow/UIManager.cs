using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public static UIManager UI;
    public GameObject MiniMap;
    public GameObject Map;
    public float MinimapCameraSize = 5;
    public float MapCameraSize = 7.5f;
    public Camera Cam;

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MiniMap.SetActive(false);
            Map.SetActive(true);
            Cam.orthographicSize = MapCameraSize;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            MiniMap.SetActive(true);
            Map.SetActive(false);
            Cam.orthographicSize = MinimapCameraSize;
        }
    }

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
}
