using System.Collections.Generic;
using UnityEngine;

public class NBodySystem : MonoBehaviour
{
    #region Singleton

    static NBodySystem _instance;
    public static NBodySystem Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<NBodySystem>();
            return _instance;
        }
    }

    #endregion

    public bool Running = true;
    public float Step = 0.01f;

    public List<Body> Bodies = new();

    // [Header("Orbits")]
    // public bool ShowOrbits;
    // public int PointCount = 1000;
    // public float OrbitStep = 0.1f;

    [Header("Debug")]
    public bool ShowVelocityVector = false;
    public float VelocityVectorScale = 10.0f;

    bool _initialized = false;

    // const float BIG_G = 6.67e-11f;
    const float BIG_G = 0.0001f;

    private void Awake()
    {
        Time.fixedDeltaTime = Step;
    }

    private void FixedUpdate()
    {
        foreach (var body in Bodies) body.UpdateVelocity(CalculateAcceleration(body.Position, body), Step);
        foreach (var body in Bodies) body.UpdatePosition(Step);
    }

    public static Vector3 CalculateAcceleration(Vector3 point, Body ignoreBody = null)
    {
        Vector3 accel = Vector3.zero;
        foreach (var body in Instance.Bodies)
        {
            if (body == ignoreBody) continue;
            float sqrDst = (body.Position - point).sqrMagnitude;
            Vector3 dir = (body.Position - point).normalized;
            accel += dir * BIG_G * body.Mass / sqrDst;
        }

        return accel;
    }

    public void Register(Body body) => Bodies.Add(body);

    private void OnDrawGizmos()
    {
        if (ShowVelocityVector)
        {
            foreach(var body in Bodies)
            {
                Gizmos.color = body.AdditionalForce.magnitude == 0 ? Color.white : Color.red;
                Gizmos.DrawLine(body.Position, body.Position + (body.Velocity * VelocityVectorScale));
            }
        }
    }
}
