using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimManager : MonoBehaviour
{
    public Vector2 OffsetFromCenter;
    public Vector2 CenterDeadZone;
    public float SpawnDistance;
    public float KillDistance;

    public float IntervalMin = 1.0f;
    public float IntervalMax = 5.0f;

    public float Speed = 100.0f;

    public List<GameObject> Spawnables = new();

    float _lastSpawn;
    float _nextSpawn;
    public List<GameObject> _spawned = new();

    private void Start()
    {
        SpawnObject();
        SetNextSpawnTime();
    }

    private void Update()
    {
        _lastSpawn += Time.deltaTime;

        if (_lastSpawn > _nextSpawn)
        {
            _lastSpawn = 0.0f;
            SetNextSpawnTime();
            SpawnObject();
        }

        foreach (var go in new List<GameObject>(_spawned))
        {
            go.transform.position += Vector3.back * Speed * Time.deltaTime;
            if (go.transform.position.z < KillDistance)
            {
                _spawned.Remove(go);
                Destroy(go);
            }
        }
    }

    private void SetNextSpawnTime()
    {
        _nextSpawn = Random.Range(IntervalMin, IntervalMax);
    }

    private void SpawnObject()
    {
        Vector3 pos = new(
            Random.Range(CenterDeadZone.x, OffsetFromCenter.x) * (Random.value > 0.5f ? -1.0f : 1.0f),
            Random.Range(CenterDeadZone.y, OffsetFromCenter.y) * (Random.value > 0.5f ? -1.0f : 1.0f),
            SpawnDistance
        );

        int objIndex = Random.Range(0, Spawnables.Count);

        GameObject go = Instantiate(Spawnables[objIndex], pos, Random.rotation);
        _spawned.Add(go);
    }
}
