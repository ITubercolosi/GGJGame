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

    public SimulatedBody Ship;
    public List<StaticBody> Bodies = new();

    bool _initialized = false;

    public const float BIG_G = 0.01f;

    private void Awake()
    {
        Time.fixedDeltaTime = Step;
    }

    private void FixedUpdate()
    {
        if (Ship.Simulate) Ship.UpdateVelocity(CalculateAcceleration(Ship.Position), Step);
        if (Ship.Simulate) Ship.UpdatePosition(Step);
    }

    public static Vector3 CalculateAcceleration(Vector3 point)
    {
        Vector3 accel = Vector3.zero;
        foreach (var body in Instance.Bodies)
        {
            float sqrDst = (body.Position - point).sqrMagnitude;
            Vector3 dir = (body.Position - point).normalized;
            accel += dir * BIG_G * body.Mass / sqrDst;
        }

        return accel;
    }

    public void RegisterAsShip(SimulatedBody body) => Ship = body;
    public void Register(StaticBody body) => Bodies.Add(body);
}
