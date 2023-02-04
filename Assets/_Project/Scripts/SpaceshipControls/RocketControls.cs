using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketControls : MonoBehaviour
{
    public float Mass = 1.0f;
    public Vector3 Velocity = Vector3.zero;
    private bool m_RocketLaunched = false;
    private float m_InitialCharge = 0.0f;
    [Header("Fuel")]
    public float Fuel = 1.0f;
    public float FuelCumsuptionSpeed;
    public float RotationSpeed = 1;
    public float BoostSpeed = 1;
    [Header("Charge")]
    float m_StartTime;
    public float Duration = 5.0f;
    private bool m_StopIntialCharge;

    [Header("Debug")]
    public MeshRenderer TopMat;
    public MeshRenderer RightMat;
    public MeshRenderer LeftMat;
    public MeshRenderer BottomMat;

    [Header("UI")]
    public Slider FuelSlider;

    public static RocketControls RocketSingleton;

    // Start is called before the first frame update
    void Start()
    {
        if (RocketSingleton == null)
        {
            RocketSingleton = this;
        }
        else
        {
            DestroyImmediate(this);
        }

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
            else
            {
                ResetDebugMaterialColor();

                if (Fuel > 0.0f)
                {
                    if (Input.GetKey(KeyCode.A)) // ROTATE
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * RotationSpeed);
                        ConsumeFuel();
                        RightMat.sharedMaterial.color = Color.red;

                    }
                    else if (Input.GetKey(KeyCode.D)) // ROTATE
                    {
                        LeftMat.sharedMaterial.color = Color.red;
                        transform.Rotate(Vector3.back * Time.deltaTime * RotationSpeed);
                        ConsumeFuel();
                    }

                    if (Input.GetKey(KeyCode.W)) // BOOST FORWARD
                    {
                        BottomMat.sharedMaterial.color = Color.red;
                        Velocity += transform.TransformDirection(Vector3.right) * Time.deltaTime * BoostSpeed;
                        ConsumeFuel();
                    }
                    else if (Input.GetKey(KeyCode.S)) // BOOST BACKWARD
                    {
                        TopMat.sharedMaterial.color = Color.red;
                        Velocity -= transform.TransformDirection(Vector3.right) * Time.deltaTime * BoostSpeed;
                        ConsumeFuel();
                    }
                }

                FuelSlider.value = Fuel;
            }
            transform.position += Velocity * Time.deltaTime;
        }
    }

    private void ResetDebugMaterialColor()
    {
        TopMat.sharedMaterial.color = Color.cyan;
        BottomMat.sharedMaterial.color = Color.cyan;
        RightMat.sharedMaterial.color = Color.cyan;
        LeftMat.sharedMaterial.color = Color.cyan;
    }

    private void ConsumeFuel()
    {
        Fuel -= FuelCumsuptionSpeed * Time.deltaTime;
        Fuel = Mathf.Clamp01(Fuel);
    }

    public IEnumerator StopChargedVelocity()
    {
        Debug.Log("controls still disabled");
        yield return new WaitForSeconds(Duration);
        m_StopIntialCharge = true;
        Debug.Log("controls enabled");
    }

    private void ApplyChargeToVelocity()
    {
        m_StartTime += Duration * Time.deltaTime;
        float verticalSpeed = Mathf.SmoothStep(Velocity.z, m_InitialCharge, m_StartTime);
        Velocity = transform.TransformDirection(new Vector3(verticalSpeed, 0, 0)) / Mass;

    }

    public void LaunchRocket(float charge)
    {
        m_RocketLaunched = true;
        m_InitialCharge = charge;
        transform.SetParent(null);
        StartCoroutine(StopChargedVelocity());
    }

    public void ApplyVelocityChangeInfluencedByObject(Vector3 origin, float attractionAmount,float radius)
    {
        Vector3 directionNormalized = (origin - transform.position).normalized;
        float distance = Vector3.Distance(origin, transform.position);
        float force = Remap(distance, 0,radius, 0, 1.0f) * attractionAmount;
        Velocity += directionNormalized * Time.deltaTime * force;
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
