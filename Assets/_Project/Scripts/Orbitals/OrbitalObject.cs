using UnityEngine;

// TODO: use real-universe mass, pos and vel for planets
// and then scale them down in a log fashion
public class OrbitalObject : MonoBehaviour
{
    // TODO: change in float and multuply by transform.forward
    public Vector3 Velocity;
    public float Mass;
    public float SphereRadius = float.PositiveInfinity;

    [Header("Debug")]
    public Color PlanetColor = Color.white;
    public float Radius = 0.25f;

    const float BIG_G = 6.67e-11f;

    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }

    private void Start()
    {
        OrbitalSystem.Instance.Register(this);
    }

    // Crude implementation of https://en.wikipedia.org/wiki/N-body_simulation
    public void OrbitalUpdate()
    {
        Vector3 ag = Vector3.zero;

        foreach (OrbitalObject oo in OrbitalSystem.Instance.GetObjects(Position))
        {
            if (oo == this) continue;

            float dist = Vector3.Distance(Position, oo.Position);
            float accel = -1.0f * BIG_G * oo.Mass / Mathf.Pow(dist, 2.0f);
            Vector3 unit = (Position - oo.Position).normalized;

            ag += accel * unit;
        }

        Velocity += ag;
        Position += Velocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = PlanetColor;
        Gizmos.DrawWireSphere(transform.position, Radius);

        if (SphereRadius < float.PositiveInfinity)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, SphereRadius);
        }
    }
}