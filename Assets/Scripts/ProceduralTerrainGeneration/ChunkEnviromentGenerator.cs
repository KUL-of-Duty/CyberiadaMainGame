using System.Collections.Generic;
using UnityEngine;

public class ChunkEnviromentGenerator : MonoBehaviour
{
    public ChunkEnviroment cEnv;
    List<Vector2> spawnPoints = new List<Vector2>();
    void Start()
    {
        spawnPoints = GetComponentInParent<PoissonDiscSampling>().points;

        foreach (var point in spawnPoints)
        {
            Instantiate(cEnv.chunkTrees[0], transform.position + new Vector3(point.x, 22, point.y), Quaternion.identity, transform);
        }
    }
}
