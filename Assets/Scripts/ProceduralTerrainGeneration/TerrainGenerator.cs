using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TerrainGenerator : NetworkBehaviour
{ 
    [SerializeField] GameObject tree;
    [Header("Terrain size options")]

    [SerializeField] int depth = 20;
    [SerializeField] int height = 512;
    [SerializeField] int width = 512;

    [Space]

    [Header("Seed options")]
    [SerializeField] int seed;
    [SerializeField] int scale = 20;
    Terrain terrain;

    [Space]

    [Header("Synchronization")]
    NetworkVariable<bool> IsWorldGenerated = new NetworkVariable<bool>(false);
    NetworkVariable<int> ServerSeed = new NetworkVariable<int>();

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

    private void Update()
    {
        if(IsWorldGenerated.Value)
        {
            seed = ServerSeed.Value;
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
            this.enabled = false;
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            seed = Random.Range(0, 99999);
            ServerSeed.Value = seed;
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
            IsWorldGenerated.Value = true;
        }
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float heightValue = CalculateHeight(x, y);

                heights[x, y] = heightValue;
            }
        }
        return heights;
    }

    private float CalculateHeight(int x, int y) 
    {
        float xCoord = (float)x / width * scale + seed;
        float yCoord = (float)y / height * scale + seed;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    [ServerRpc]
    public void WorldGeneratedServerRPC(bool value)
    {
        IsWorldGenerated.Value = value;
    }

    [ServerRpc]
    public void SendSeedServerRPC(int seed)
    {
        ServerSeed.Value = seed;
    }
}