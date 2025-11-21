using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{ 
    [SerializeField] GameObject tree;
    [Header("Terrain size options")]

    [SerializeField] int depth;
    [SerializeField] int height;
    [SerializeField] int width;
    [SerializeField] float yOffSet;
    int xChunkOffset;
    int yChunkOffset;


    [Space]

    [Header("Seed options")]
    [SerializeField] int seed;
    [SerializeField] int scale;
    Terrain terrain;

    private void Awake()
    {

        terrain = GetComponent<Terrain>();
    }

    public void InitalizeTerrainGenerator(int generatedSeed, int i, int j)
    {
        seed = generatedSeed;
        xChunkOffset = i * width;
        yChunkOffset = j * height;
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
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
        float worldX = (x + seed + xChunkOffset); 
        float worldY = (y + seed + yChunkOffset); 
        float xCoord = (worldX) / scale;
        float yCoord = (worldY) / scale;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}