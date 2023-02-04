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

    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }

    private void Start()
    {
        OrbitalSystem.Instance.Register(this);
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