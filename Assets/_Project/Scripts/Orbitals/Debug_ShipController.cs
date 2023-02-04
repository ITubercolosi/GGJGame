using UnityEngine;

[RequireComponent(typeof(Body))]
public class Debug_ShipController : MonoBehaviour
{
    public float Acceleration = 0.0f;

    Body _body;

    private void Start()
    {
        _body = GetComponent<Body>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) _body.AdditionalForce = _body.Forward * Acceleration;
        else if (Input.GetKey(KeyCode.S)) _body.AdditionalForce = -1.0f * _body.Forward * Acceleration;
        else _body.AdditionalForce = Vector3.zero;
    }
}