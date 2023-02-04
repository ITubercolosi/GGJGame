using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(LineRenderer))]
public class EllipticBody : Body
{
    [Header("Elliptic Body")]
    public Vector3 CenterOffset;
    public float SemiMinorAxis;
    public Body OrbitAround;
    public float Speed = 1.0f;

    [Header("Debug")]
    public int Segments = 180;

    [Header("Debug")]
    public bool ShowEllipseGizmo = false;

    LineRenderer _lr;

    float _t = 0.0f;
    float _semiMajorAxis;
    Vector3 _center;
    float _theta;
    Vector3 _initialPosition;
    Quaternion _rotation;

    List<Vector3> _points = new();

    private new void Start()
    {
        base.Start();

        GetReferences();

        CalculateParameters();
        GenerateOrbit();
        UpdateRenderer();
    }

    public void GetReferences()
    {
        if (_lr == null) _lr = GetComponent<LineRenderer>();
    }

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

    public void CalculateParameters()
    {
        _initialPosition = transform.position;
        _center = OrbitAround.Position + CenterOffset;
        _semiMajorAxis = Vector3.Distance(_center, transform.position);
        _theta = Mathf.Rad2Deg * (Mathf.Atan2(transform.position.z - _center.z, transform.position.x - _center.x));
        _rotation = Quaternion.AngleAxis(-_theta, Vector3.up);
    }

    public void GenerateOrbit()
    {
        _points.Clear();
        for (int i = 0; i < Segments; i++)
        {
            float angle = (float)i / (float)Segments * 2.0f * Mathf.PI;
            Vector3 pos = new Vector3(
                _semiMajorAxis * Mathf.Cos(angle),
                0.0f,
                SemiMinorAxis * Mathf.Sin(angle)
            );
            
            _points.Add(_center + _rotation * pos);
        }
    }

    public void UpdateRenderer()
    {
        _lr.positionCount = _points.Count;
        _lr.SetPositions(_points.ToArray());
    }

    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (ShowEllipseGizmo)
        {
            if (!Application.isPlaying)
            {
                CalculateParameters();
                GenerateOrbit();
            }
            
            Handles.DrawAAPolyLine(_points.ToArray());
        }
    }
}