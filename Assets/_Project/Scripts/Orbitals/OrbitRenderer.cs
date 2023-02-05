using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(SimulatedBody))]
public class OrbitRenderer : MonoBehaviour
{
    public float MaxDistance = 100.0f;
    public float TimeStep = 0.01f;

    List<Vector3> _points = new();

    SimulatedBody _body;
    VirtualBody _vbody;

    void Start()
    {
        _body = GetComponent<SimulatedBody>();
    }

    public void SimulateTrajectory()
    {
        if (!Application.isPlaying) _body = GetComponent<SimulatedBody>();

        _vbody = new VirtualBody(_body);
        _points.Clear();
        Vector3 lastPoint = _vbody.Position;
        _points.Add(_body.Position);

        float dist = 0;
        int iters = 0;

        while (dist < MaxDistance && iters < 10000)
        {
            Vector3 accel = Vector3.zero;
            foreach (var body in NBodySystem.Instance.Bodies)
            {
                float sqrDst = (body.Position - _vbody.Position).sqrMagnitude;
                Vector3 dir = (body.Position - _vbody.Position).normalized;
                accel += dir * NBodySystem.BIG_G * body.Mass / sqrDst;
            }

            _vbody.UpdateVelocity(accel, TimeStep);
            _vbody.UpdatePosition(TimeStep);

            _points.Add(_vbody.Position);
            dist += Vector3.Distance(_vbody.Position, lastPoint);
            lastPoint = _vbody.Position;

            iters++;
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawAAPolyLine(3.0f, _points.ToArray());
    }

    class VirtualBody
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 AdditionalForce;
        public float Mass;

        public VirtualBody(SimulatedBody body)
        {
            Position = body.Position;
            Velocity = body.Velocity;
            AdditionalForce = body.AdditionalForce;
            Mass = body.Mass;
        }

        public void UpdateVelocity(Vector3 accel, float dt)
        {
            Velocity += accel * dt;
        }

        public void UpdatePosition(float dt)
        {
            Position += Velocity * dt;
        }
    }
}
