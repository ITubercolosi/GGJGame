using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

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
    private SimulatedBody SimBody;
    private Rigidbody m_RB;
    private Vector3 m_StartPos;
    private int m_Score;

    public EventInstance ThrusterNavTrack;

    public EventReference ThrusterNavStateEvent;
    // Start is called before the first frame update
    private bool m_OutOfFuel;

    void Start()
    {
        ThrusterNavTrack = FMODUnity.RuntimeManager.CreateInstance(ThrusterNavStateEvent);

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

        SimBody = GetComponent<SimulatedBody>();
        SimBody.DisableSimulation();
        m_RB = GetComponent<Rigidbody>();
        m_StartPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_OutOfFuel)
        {
            return;
        }

        if (m_RocketLaunched)
        {
            if (!m_StopIntialCharge)
            {
                ApplyChargeToVelocity();
                SimBody.AdditionalForce = Velocity;
            }
            else
            {
                SimBody.AdditionalForce = Vector3.zero;

                ResetDebugMaterialColor();
                Velocity = Vector3.zero;

                if (Fuel > 0.0f)
                {
                    bool playSound = false;

                    if (Input.GetKey(KeyCode.LeftArrow)) // ROTATE
                    {
                        RightMat.sharedMaterial.color = Color.red;
                        Quaternion targetRotation = Quaternion.LookRotation(transform.TransformDirection(Vector3.right), transform.TransformDirection(Vector3.up));
                        m_RB.MoveRotation(Quaternion.Slerp(m_RB.rotation, targetRotation, Time.deltaTime * RotationSpeed));
                        playSound = true;
                        ConsumeFuel();

                    }
                    else if (Input.GetKey(KeyCode.RightArrow)) // ROTATE
                    {
                        LeftMat.sharedMaterial.color = Color.red;
                        Quaternion targetRotation = Quaternion.LookRotation(transform.TransformDirection(Vector3.left), transform.TransformDirection(Vector3.up));
                        m_RB.MoveRotation(Quaternion.Slerp(m_RB.rotation, targetRotation, Time.deltaTime * RotationSpeed));
                        playSound = true;
                        ConsumeFuel();
                    }

                    if (Input.GetKey(KeyCode.S)) // BOOST FORWARD
                    {
                        BottomMat.sharedMaterial.color = Color.red;
                        Velocity = transform.TransformDirection(Vector3.forward) * BoostSpeed;
                        ConsumeFuel();
                        playSound = true;
                    }
                    if (Input.GetKey(KeyCode.W)) // BOOST BACKWARD
                    {
                        TopMat.sharedMaterial.color = Color.red;
                        Velocity = transform.TransformDirection(Vector3.back) * BoostSpeed;
                        ConsumeFuel();
                        playSound = true;
                    }
                    if (Input.GetKey(KeyCode.A)) // BOOST RIGHT
                    {
                        RightMat.sharedMaterial.color = Color.red;
                        Velocity = transform.TransformDirection(Vector3.left) * BoostSpeed;
                        ConsumeFuel();
                        playSound = true;
                    }
                    if (Input.GetKey(KeyCode.D)) // BOOST LEFT
                    {
                        LeftMat.sharedMaterial.color = Color.red;
                        Velocity = transform.TransformDirection(Vector3.right) * BoostSpeed;
                        ConsumeFuel();
                        playSound = true;
                    }

                    if (playSound) 
                    {
                        PLAYBACK_STATE state;
                        ThrusterNavTrack.getPlaybackState(out state);
                        if (state != PLAYBACK_STATE.PLAYING) ThrusterNavTrack.start();
                    }
                    else ThrusterNavTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }
                else
                {
                    ThrusterNavTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    m_OutOfFuel = true;
                    GameOver();
                    UIManager.UI.ShowGameOverPanelOutOfFuel();
                }

                FuelSlider.value = Fuel;
            }
            SimBody.AdditionalForce = Velocity;
        }

        m_Score = Mathf.Max((int)Vector3.Distance(transform.position, m_StartPos), m_Score);
        UIManager.UI.SetScoreText(m_Score);

        if (Input.GetKeyDown(KeyCode.Alpha3)) NBodySystem.Instance.SpeedUp();
        if (Input.GetKeyDown(KeyCode.Alpha1)) NBodySystem.Instance.SpeedDown();
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
        SimBody.AdditionalForce = Vector3.zero;
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
        SimBody.EnableSimulation();
        m_RocketLaunched = true;
        Vector3 initialCharge = transform.TransformDirection(Vector3.forward * charge);
        transform.SetParent(null);
        SimBody.AdditionalForce = initialCharge;
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

    public void GameOver()
    {
        SimBody.Simulate = false;
        enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Finish" && m_Score > 5) // valore indicativo
        {
            GameOver();
            UIManager.UI.ShowWinPanel();
        }

        if (other.gameObject.tag == "Collision") // valore indicativo
        {
            GameOver();
            UIManager.UI.ShowGameOverPanelDestroyed();
        }
    }
}
