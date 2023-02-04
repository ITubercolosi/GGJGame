using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderGravity : MonoBehaviour
{
    public float Radius = 1;
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
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = DebugColor;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
