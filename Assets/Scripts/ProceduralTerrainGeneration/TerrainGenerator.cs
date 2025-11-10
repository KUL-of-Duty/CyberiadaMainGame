using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{ 
    [SerializeField] GameObject tree;
    [Header("Terrain size options")]

    [SerializeField] int depth = 20;
    [SerializeField] int height = 512;
    [SerializeField] int width = 512;

    [SerializeField] float yOffSet = 10;

    [Space]

    [Header("Seed options")]
    [SerializeField] int seed;
    [SerializeField] int scale = 20;
    Terrain terrain;

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

    public IEnumerator InitalizeTerrainGenerator()
    {
        seed = gameObject.GetComponent<SeedGenerator>().ServerSeed.Value;
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        yield return new WaitForSeconds(1f);
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

    public IEnumerator GenerateTrees()
    {
        List<Vector2> treeLocations = GetComponent<PoissonDiscSampling>().points;

        foreach (Vector2 location in treeLocations)
        {
            Vector3 samplePos = new Vector3(
                location.x + terrain.transform.position.x,
                0,
                location.y + terrain.transform.position.z
            );

            float worldHeight = terrain.SampleHeight(samplePos);

            Vector3 treeLocation = new Vector3(
                samplePos.x,
                worldHeight + yOffSet,
                samplePos.z
            );

            Instantiate(tree, treeLocation, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }

    private float CalculateHeight(int x, int y) 
    {
        float xCoord = (float)x / width * scale + seed;
        float yCoord = (float)y / height * scale + seed;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}