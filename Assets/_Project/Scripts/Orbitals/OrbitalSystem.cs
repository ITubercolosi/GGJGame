using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Threading;

struct Body
{
    public Transform Ref;
    public Vector3 Position;
    public Vector3 Velocity;
    public float Mass;
}

public class OrbitalSystem : MonoBehaviour
{
    #region Singleton

    static OrbitalSystem _instance;
    public static OrbitalSystem Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<OrbitalSystem>();
            return _instance;
        }
    }

    #endregion

    public bool Running = true;
    public float Step = 0.1f;

    public List<OrbitalObject> Objects = new();

    bool _initialized = false;
    Body[] _bodies;
    Thread _simThread;

    const float BIG_G = 6.67e-11f;

    public void InitializeSimulation()
    {
        _bodies = new Body[Objects.Count];
        for (int i = 0; i < Objects.Count; i++)
        {
            _bodies[i] = new(){
                Ref = Objects[i].transform,
                Position = Objects[i].Position,
                Velocity = Objects[i].Velocity,
                Mass = Objects[i].Mass
            };
        }

        _initialized = true;

        _simThread = new(Simulate);
        _simThread.Start();
    }

    private void Update()
    {
        if (_initialized)
        {
            foreach (Body body in _bodies)
            {
                body.Ref.position = body.Position;
            }
        }
    }

    public void Register(OrbitalObject oobj) => Objects.Add(oobj);

    // Crude implementation of https://en.wikipedia.org/wiki/N-body_simulation
    void Simulate()
    {
        Stopwatch sw = new();
        sw.Start();

        while (true)
        {
            if (!Running || !_initialized) continue;
            if ((float)sw.Elapsed.Ticks / (float)TimeSpan.TicksPerSecond < Step) continue;
            sw.Restart();

            lock (_bodies)
            {
                for (int i = 0; i < _bodies.Length; i++)
                {
                    Vector3 ag = Vector3.zero;

                    for (int j = 0; j < _bodies.Length; j++)
                    {
                        if (i == j) continue;

                        Body ba = _bodies[i];
                        Body bb = _bodies[j];

                        float dist = Vector3.Distance(ba.Position, bb.Position);
                        float accel = -1.0f * BIG_G *  bb.Mass / (dist * dist);
                        Vector3 dir = (ba.Position - bb.Position).normalized;
                        ag += accel * dir;
                    }

                    _bodies[i].Velocity += ag;
                    _bodies[i].Position += _bodies[i].Velocity;
                }
            }
        }
    }
}
