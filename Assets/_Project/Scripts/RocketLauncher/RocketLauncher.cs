using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [Range(1, 30)]
    public float angularAcceleration = 1f;
    [Range(90,160)]
    public float angularLimitLeft = 160f;
    [Range(20,90)]
    public float angularLimitRight = 20f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, 90);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            transform.Rotate(Vector3.forward * Input.GetAxis("Horizontal") * angularAcceleration/10 , Space.Self);
        }
        if (transform.localEulerAngles.z > angularLimitLeft)
        {
            transform.localEulerAngles = new Vector3(0, 0, angularLimitLeft);
        }
        else if (transform.localEulerAngles.z < angularLimitRight)
        {
            transform.localEulerAngles = new Vector3(0, 0, angularLimitRight);
        }
    }
}