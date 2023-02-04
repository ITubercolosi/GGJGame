using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public float minRotation = -35;
    public float maxRotation = 35;

    private void Start()
    {
        this.transform.rotation = Quaternion.Euler(Random.RandomRange(minRotation, maxRotation), 0, 0);
    }

    private void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10f);
    }
}
