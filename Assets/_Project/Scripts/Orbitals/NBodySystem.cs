using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    // public bool ShowOrbits = false;
    // public int PointCount = 1000;
    // public float OrbitStep = 0.1f;
    // public bool RelativeToBody = false;
    // public Body CentralBody;

    bool _initialized = false;
    // Vector3[][] _points;

    const float BIG_G = 0.01f;

    private void Awake()
    {
        Time.fixedDeltaTime = Step;
    }

    private void FixedUpdate()
    {
        foreach (var body in Bodies) if (body.Simulate) body.UpdateVelocity(CalculateAcceleration(body.Position, body), Step);
        foreach (var body in Bodies) if (body.Simulate) body.UpdatePosition(Step);
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

    // private void OnDrawGizmos()
    // {
    //     if (ShowOrbits && _points != null)
    //     {
    //         Handles.color = Color.white;
    //         foreach(var orbit in _points) Handles.DrawAAPolyLine(orbit);
    //     }
    // }

    #region Orbits

    // private void CalculateOrbits()
    // {
    //     var virtualBodies = new VirtualBody[Bodies.Count];
    //     _points = new Vector3[Bodies.Count][];

    //     int refBodyIndex = 0;
    //     Vector3 refBodyInitialPos = Vector3.zero;

    //     for (int i = 0; i < Bodies.Count; i++)
    //     {
    //         virtualBodies[i] = new VirtualBody(Bodies[i]);
    //         _points[i] = new Vector3[PointCount];

    //         if (RelativeToBody && Bodies[i] == CentralBody)
    //         {
    //             refBodyIndex = i;
    //             refBodyInitialPos = virtualBodies[i].Position;
    //         }
    //     }

    //     for (int step = 0; step < PointCount; step++)
    //     {
    //         Vector3 refBodyPos = RelativeToBody ? virtualBodies[refBodyIndex].Position : Vector3.zero;

    //         foreach (var vbody in virtualBodies)
    //         {
    //             Vector3 accel = Vector3.zero;

    //             foreach (var otherBody in virtualBodies)
    //             {
    //                 if (vbody == otherBody) continue;
    //                 Vector3 forceDir = (otherBody.Position - vbody.Position).normalized;
    //                 float sqrDst = (otherBody.Position - vbody.Position).sqrMagnitude;
    //                 accel += forceDir * BIG_G * otherBody.Mass / sqrDst;
    //             }

    //             vbody.Velocity += accel * OrbitStep;
    //         }

    //         for (int i = 0; i < virtualBodies.Length; i++)
    //         {
    //             Vector3 newPos = virtualBodies[i].Position + virtualBodies[i].Velocity * OrbitStep;
    //             virtualBodies[i].Position = newPos;

    //             if (RelativeToBody) newPos -= refBodyPos - refBodyInitialPos;
    //             if (RelativeToBody && i == refBodyIndex) newPos = refBodyInitialPos;

    //             _points[i][step] = newPos;
    //         }
    //     }
    // }

    // class VirtualBody
    // {
    //     public Vector3 Position;
    //     public Vector3 Velocity;
    //     public float Mass;

    //     public VirtualBody(Body body)
    //     {
    //         Position = body.transform.position;
    //         Velocity = body.Velocity;
    //         Mass = body.Mass;
    //     }
    // }

    #endregion
}
