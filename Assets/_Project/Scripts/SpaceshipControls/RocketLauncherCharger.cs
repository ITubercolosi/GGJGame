using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

public class RocketLauncherCharger : MonoBehaviour
{
    public RocketControls Rocket;
    public RocketLauncher Rotator;
    public float RocketCharge = 0.0f;
    public float RocketChargeDelta = 1.0f;
    public float RocketChargePowerMultiplier = 1.0f;
    public Slider RocketChargeSlider;
    private bool m_RocketLauched;
    [Space(10)]
    public EventInstance RocketTrusterTrack;

    public EventReference RocketTrusterStateEvent;

    private void Start()
    {
        RocketTrusterTrack = FMODUnity.RuntimeManager.CreateInstance(RocketTrusterStateEvent);
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_RocketLauched = true;
            Rocket.LaunchRocket(RocketCharge * RocketChargePowerMultiplier);
            enabled = false;
            Rotator.enabled = false;
            RocketTrusterTrack.start();

        }

        if (!m_RocketLauched)
        {
            ChargeRocketLauncher();
        }
    }

    private void ChargeRocketLauncher()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            RocketCharge += Time.deltaTime * RocketChargeDelta;
        }
        else
        {
            RocketCharge -= Time.deltaTime * RocketChargeDelta;
        }
        RocketCharge = Mathf.Clamp01(RocketCharge);
        RocketChargeSlider.value = RocketCharge;
    }
}
