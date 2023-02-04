using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderGravity : MonoBehaviour
{
    public float Radius = 1;
    public float DeadZoneRadius = 1;
    public float PlaceholderAttractionThreshold = 1;
    [Header("Debug")]
    public Color DebugColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(RocketControls.RocketSingleton.transform.position, transform.position) < Radius)
        {
            RocketControls.RocketSingleton.ApplyVelocityChangeInfluencedByObject(transform.position, PlaceholderAttractionThreshold, Radius);

            if (Vector3.Distance(RocketControls.RocketSingleton.transform.position, transform.position) < DeadZoneRadius)
            {
                RocketControls.RocketSingleton.RocketInDeadZone();
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = DebugColor;
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, Mathf.Clamp(DeadZoneRadius, 0, Radius));

    }
}
