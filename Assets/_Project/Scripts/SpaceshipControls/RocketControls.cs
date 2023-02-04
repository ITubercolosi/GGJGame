using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControls : MonoBehaviour
{
    public float Mass = 1.0f;
    public Vector3 Velocity = Vector3.zero;
    private bool m_RocketLaunched = false;
    private float m_InitialCharge = 0.0f;

    [Header("Debug")]
    public MeshRenderer TopMat;
    public MeshRenderer RightMat;
    public MeshRenderer LeftMat;
    public MeshRenderer BottomMat;

    // Start is called before the first frame update
    void Start()
    {
        TopMat.sharedMaterial = TopMat.material;
        RightMat.sharedMaterial = RightMat.material;
        LeftMat.sharedMaterial = LeftMat.material;
        BottomMat.sharedMaterial = BottomMat.material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_RocketLaunched)
        {
            ApplyChargeToVelocity();
        }
    }

    float startTime;
    public float duration = 5.0f;
    private void ApplyChargeToVelocity()
    {
        startTime += duration * Time.deltaTime;
        float verticalSpeed = Mathf.SmoothStep(Velocity.z, m_InitialCharge, startTime);
        Velocity = transform.TransformDirection(new Vector3(0,0, verticalSpeed)) / Mass;
        transform.position += Velocity;
        
    }

    public void LaunchRocket(float charge)
    {
        m_RocketLaunched = true;
        m_InitialCharge = charge;
        transform.SetParent(null);
    }
}
