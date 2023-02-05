using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FMOD.Studio;
using FMODUnity;

public class ZoomInOut : MonoBehaviour
{

    private CinemachineCameraOffset m_VirtualCam;
    public float ZoomSpeed;
    public float ZoomGame = 0;
    public float ZoomTop = 10;

    public EventInstance RadarTrack;

    public EventReference RadarTrackStateEvent;

    // Start is called before the first frame update
    void Start()
    {
        m_VirtualCam = GetComponent<CinemachineCameraOffset>();
        RadarTrack = FMODUnity.RuntimeManager.CreateInstance(RadarTrackStateEvent);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
                m_VirtualCam.m_Offset.z -= ZoomSpeed * Time.deltaTime;           
                PLAYBACK_STATE state;
                RadarTrack.getPlaybackState(out state);
                if (state != PLAYBACK_STATE.PLAYING) RadarTrack.start();                      
        }
        else if (!Input.GetKey(KeyCode.Tab))
        {
            RadarTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            m_VirtualCam.m_Offset.z += ZoomSpeed * Time.deltaTime;
        }

        m_VirtualCam.m_Offset.z = Mathf.Clamp(m_VirtualCam.m_Offset.z, ZoomTop, ZoomGame);
    }
}
