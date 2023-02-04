using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControls : MonoBehaviour
{
    public float Mass = 1.0f;
    public Vector3 Velocity = Vector3.zero;
    private bool m_RocketLaunched = false;
    private float m_InitialCharge = 0.0f;

    float m_StartTime;
    public float Duration = 5.0f;
    private bool m_StopIntialCharge;

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
            if (!m_StopIntialCharge)
            {
                ApplyChargeToVelocity();
            }
            transform.position += Velocity * Time.deltaTime;
        }
    }

    public IEnumerator StopChargedVelocity()
    {
        yield return new WaitForSeconds(Duration);
        m_StopIntialCharge = true;
    }

    private void ApplyChargeToVelocity()
    {
        m_StartTime += Duration * Time.deltaTime;
        float verticalSpeed = Mathf.SmoothStep(Velocity.z, m_InitialCharge, m_StartTime);
        Velocity = transform.TransformDirection(new Vector3(0,0, verticalSpeed)) / Mass;      
        
    }

    public void LaunchRocket(float charge)
    {
        m_RocketLaunched = true;
        m_InitialCharge = charge;
        transform.SetParent(null);
        StartCoroutine(StopChargedVelocity());
    }
}
