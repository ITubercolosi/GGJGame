using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
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
            transform.Rotate(Vector3.back * Input.GetAxis("Horizontal"), Space.Self);
        }
        if (transform.localEulerAngles.z > 160)
        {
            transform.localEulerAngles = new Vector3(0, 0, 160);
        }
        else if (transform.localEulerAngles.z < 20)
        {
            transform.localEulerAngles = new Vector3(0, 0, 20);
        }
    }
}