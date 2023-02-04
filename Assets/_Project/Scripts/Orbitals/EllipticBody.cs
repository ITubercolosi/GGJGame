using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class EllipticBody : Body
{
    [Header("Elliptic Body")]
    public Vector3 CenterOffset;
    public float SemiMinorAxis;
    public Body OrbitAround;
    public float Speed = 1.0f;

    [Header("Debug")]
    public bool ShowEllipse = false;
    public int Segments = 256;

    List<Vector3> positions;
    float _semiMajorAxis;
    Vector3 _center;
    public float _theta;
    public Vector3 _initialPosition;
    Quaternion _rotation;

    float _t = 0.0f;

    public override void UpdateVelocity(Vector3 accel, float dt)
    {
        _center = OrbitAround.Position + CenterOffset;
    }

    public override void UpdatePosition(float dt)
    {
        _t = (_t + Speed * dt) % 1;
        float angle = _t * 2.0f * Mathf.PI;

        transform.position = _rotation * new Vector3(
            _semiMajorAxis * Mathf.Cos(angle),
            0.0f,
            SemiMinorAxis * Mathf.Sin(angle)
        ) + _center;
    }

    private new void Start()
    {
        base.Start();

        _initialPosition = transform.position;
        _center = OrbitAround.Position + CenterOffset;
        _semiMajorAxis = Vector3.Distance(_center, transform.position);
        _theta = Mathf.Rad2Deg * (Mathf.Atan2(transform.position.z - _center.z, transform.position.x - _center.x));
        _rotation = Quaternion.AngleAxis(-_theta, Vector3.up);
    }

    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (ShowEllipse)
        {
            Vector3 bodyPos = Application.isPlaying ? _initialPosition : transform.position;

            Vector3 center = OrbitAround.Position + CenterOffset;
            float semiMajorAxis = Vector3.Distance(_center, bodyPos);
            float theta = Mathf.Rad2Deg * (Mathf.Atan2(bodyPos.z - _center.z, bodyPos.x - _center.x));

            positions = new List<Vector3>();
            Quaternion rot = Quaternion.AngleAxis(-theta, Vector3.up);

            for (int i = 0; i < Segments; i++)
            {
                float angle = (float)i / (float)Segments * 2.0f * Mathf.PI;
                Vector3 pos = new Vector3(
                    semiMajorAxis * Mathf.Cos(angle),
                    0.0f,
                    SemiMinorAxis * Mathf.Sin(angle)
                );
                
                positions.Add(center + rot * pos);
            }

            Handles.DrawAAPolyLine(positions.ToArray());
        }
    }
}