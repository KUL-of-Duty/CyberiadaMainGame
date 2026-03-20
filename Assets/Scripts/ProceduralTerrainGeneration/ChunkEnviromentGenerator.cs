using System.Collections.Generic;
using UnityEngine;

public class ChunkEnviromentGenerator : MonoBehaviour
{
    public ChunkEnviroment cEnv;
    public ChunkType type;
    public enum ChunkType
    {
        Plains,
        Church,
        Hut
    }
    List<Vector2> spawnPoints = new List<Vector2>();
    void Start()
    {
        spawnPoints = GetComponentInParent<PoissonDiscSampling>().points;
        int mushroomsToSpawn = cEnv.minMushrooms;

        for(int i = 0; i < (cEnv.maxMushrooms - cEnv.minMushrooms); i++)
        {
            if (Random.Range(0, 100) <= cEnv.multipleMushroomsChance) mushroomsToSpawn++;
        }

        for (int i = 0; i < mushroomsToSpawn; i++)
        {
            int mushroomType = Random.Range(0, cEnv.mushroomsPossible.Length);
            Instantiate(cEnv.mushroomsPossible[mushroomType], transform.position + Vector3.right * Random.Range(0, 256) + Vector3.forward * Random.Range(0, 256) + Vector3.up * 20, Quaternion.identity, transform);
        }

        switch (type)
        {
            case ChunkType.Plains:
                foreach (var point in spawnPoints)
                {
                    Instantiate(cEnv.chunkTrees[Random.Range(0, cEnv.chunkTrees.Length)], transform.position + new Vector3(point.x, 20, point.y), Quaternion.identity, transform);
                }
                break;
            case ChunkType.Church:

                break;
            case ChunkType.Hut:

                break;

        }
    }
}
