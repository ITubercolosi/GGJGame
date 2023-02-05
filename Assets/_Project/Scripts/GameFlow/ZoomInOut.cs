using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomInOut : MonoBehaviour
{

    private CinemachineCameraOffset m_VirtualCam;
    public float ZoomSpeed;
    public float ZoomGame = 0;
    public float ZoomTop = 10;
    // Start is called before the first frame update
    void Start()
    {
        m_VirtualCam = GetComponent<CinemachineCameraOffset>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            m_VirtualCam.m_Offset.z -= ZoomSpeed * Time.deltaTime;
        }
        else if (!Input.GetKey(KeyCode.Tab))
        {
            m_VirtualCam.m_Offset.z += ZoomSpeed * Time.deltaTime;
        }

        m_VirtualCam.m_Offset.z = Mathf.Clamp(m_VirtualCam.m_Offset.z, ZoomTop, ZoomGame);
    }
}
