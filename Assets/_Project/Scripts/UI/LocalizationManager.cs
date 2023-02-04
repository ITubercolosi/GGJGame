using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
    public TextMeshPro text;
    public bool Eng;

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
        if (Eng)
        {
            text.text = "ITA";

        }
        else
        {
            text.text = "ENG";
        }
    }
}
