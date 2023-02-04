using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncherCharger : MonoBehaviour
{
    private RocketControls m_Rocket;
    public float RocketCharge = 0.0f;
    public float RocketChargeDelta = 1.0f;
    public Slider RocketChargeSlider;
    private bool m_RocketLauched;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_RocketLauched = true;
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
