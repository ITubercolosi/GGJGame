using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalSystem : MonoBehaviour
{
    static OrbitalSystem _instance;
    public static OrbitalSystem Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<OrbitalSystem>();
            return _instance;
        }
    }

    public bool Running = true;
    public float Step = 0.1f;

    public List<OrbitalObject> Objects = new();

    float _totalTime = 0;
    float _currentTime = 0;

    private void Update()
    {
        if (Running)
        {
            _totalTime += Time.deltaTime;
            _currentTime += Time.deltaTime;
        }
        
        if (_currentTime > Step)
        {
            OrbitalUpdate();
            _currentTime -= Step;
        }
    }

    public void OrbitalUpdate()
    {
        foreach (OrbitalObject oo in Objects) oo.OrbitalUpdate();
    }

    public void Register(OrbitalObject oobj) => Objects.Add(oobj);

    public IEnumerable<OrbitalObject> GetObjects(Vector3 position, float sphereRadius = float.PositiveInfinity)
    {
        foreach (OrbitalObject oo in Objects)
        {
            if (Vector3.Distance(oo.Position, position) < sphereRadius) yield return oo;
        }
    }
}
