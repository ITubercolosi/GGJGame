using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomInOut : MonoBehaviour
{

    private CinemachineFollowZoom m_VirtualCam;
    public float ZoomSpeed;
    // Start is called before the first frame update
    void Start()
    {
        m_VirtualCam = GetComponent<CinemachineFollowZoom>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            m_VirtualCam.m_Width += ZoomSpeed * Time.deltaTime;
        }
        else if (!Input.GetKey(KeyCode.Tab))
        {
            m_VirtualCam.m_Width -= ZoomSpeed * Time.deltaTime;
        }

        m_VirtualCam.m_Width = Mathf.Clamp(m_VirtualCam.m_Width, 0, 60);
    }
}
