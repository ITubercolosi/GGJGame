using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Body : MonoBehaviour
{
    [Header("Common")]
    public float Mass;

    [Header("Common Debug")]
    public Color PlanetColor = Color.white;
    public float Radius = 0.25f;

    protected Rigidbody _rb;

    public Vector3 Position
    {
        get => _rb != null ? _rb.position : Vector3.zero;
    }

    protected void Start()
    {
        NBodySystem.Instance.Register(this);
        _rb = GetComponent<Rigidbody>();
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = PlanetColor;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    public abstract void UpdateVelocity(Vector3 accel, float dt);
    public abstract void UpdatePosition(float dt);
}