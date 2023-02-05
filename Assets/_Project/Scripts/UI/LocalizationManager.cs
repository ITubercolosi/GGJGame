using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool Eng;

    public GameObject[] ItaTexts = new GameObject[0];
    public GameObject[] EngTexts = new GameObject[0];

    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchLanguage()
    {
        Eng = !Eng;
        if (Eng)
        {
            text.text = "ITA";
            //for (int i = 0; i < ItaTexts.Length; i++)
            //{
            //    ItaTexts[i].SetActive(!Eng);
            //}
            //for (int i = 0; i < EngTexts.Length; i++)
            //{
            //    EngTexts[i].SetActive(Eng);
            //}

        }
        else
        {
            text.text = "ENG";
        }
        for (int i = 0; i < ItaTexts.Length; i++)
        {
            ItaTexts[i].SetActive(!Eng);
        }
        for (int i = 0; i < EngTexts.Length; i++)
        {
            EngTexts[i].SetActive(Eng);
        }
    }
}
