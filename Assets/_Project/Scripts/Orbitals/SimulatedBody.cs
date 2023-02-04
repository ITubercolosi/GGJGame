using UnityEngine;

public class SimulatedBody : Body
{
    [Header("Simulated Body")]
    public Vector3 Velocity;
    public Vector3 AdditionalForce;
    
    [Header("Debug")]
    public bool ShowVelocityVector;
    public float VelocityVectorScale = 2.0f;

    public Vector3 Forward 
    {
        get => Velocity.normalized;
    }

    public override void UpdateVelocity(Vector3 accel, float dt)
    {
        Velocity += (accel + AdditionalForce) * dt;
    }

    public override void UpdatePosition(float dt)
    {
        _rb.MovePosition(_rb.position + Velocity * dt);
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