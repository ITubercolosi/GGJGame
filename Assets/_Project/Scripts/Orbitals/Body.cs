using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Body : MonoBehaviour
{
    public Vector3 Velocity;
    public Vector3 AdditionalForce;
    public float Mass;

    [Header("Debug")]
    public Color PlanetColor = Color.white;
    public float Radius = 0.25f;

    Rigidbody _rb;

    const float BIG_G = 6.67e-11f;

    public Vector3 Forward 
    {
        get => Velocity.normalized;
    }

    public Vector3 Position {
        get => _rb.position;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        NBodySystem.Instance.Register(this);
    }

    public void UpdateVelocity(Vector3 accel, float dt)
    {
        Velocity += (accel + AdditionalForce) * dt;
    }

    public void UpdatePosition(float dt)
    {
        _rb.MovePosition(_rb.position + Velocity * dt);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = PlanetColor;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}