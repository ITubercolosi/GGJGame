using UnityEngine;

public class StaticBody : Body
{
    private new void Start()
    {
        base.Start();
        NBodySystem.Instance.Register(this);
    }

    public override void UpdatePosition(float dt) {}

    public override void UpdateVelocity(Vector3 accel, float dt) {}
}