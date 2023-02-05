using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public EventInstance FullGameTrack;

    public EventReference FullGameTrackStateEvent;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
            return;
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Start()
    {
        FullGameTrack = FMODUnity.RuntimeManager.CreateInstance(FullGameTrackStateEvent);
        FullGameTrack.setParameterByName("GameStatus", (float)0);

        FullGameTrack.start();
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    FullGameTrack.setParameterByName("GameStatus", (float)1);
        //}
    }    
}
