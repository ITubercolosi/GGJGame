using UnityEngine;

public class SimulatedBody : Body
{
    [Header("Simulated Body")]
    public Vector3 Velocity;
    public Vector3 AdditionalForce;
    
    [Header("Debug")]
    public bool ShowVelocityVector;
    public float VelocityVectorScale = 2.0f;

    OrbitRenderer _or;

    public Vector3 Forward 
    {
        get => Velocity.normalized;
    }

    private new void Start()
    {
        base.Start();
        NBodySystem.Instance.RegisterAsShip(this);

        _or = GetComponent<OrbitRenderer>();
    }

    public override void UpdateVelocity(Vector3 accel, float dt)
    {
        Velocity += (accel + AdditionalForce) * dt;
    }

    public override void UpdatePosition(float dt)
    {
        _rb.MovePosition(_rb.position + Velocity * dt);

        if (_or != null) _or.SimulateTrajectory();
    }

    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (ShowVelocityVector)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position + Velocity.normalized * VelocityVectorScale);
        }
    }
}