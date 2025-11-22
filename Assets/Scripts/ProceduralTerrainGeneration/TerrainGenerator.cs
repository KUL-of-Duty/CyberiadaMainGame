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
    [SerializeField] int octaves;
    [SerializeField] float lucunarity;
    [SerializeField] float persistance;
    Terrain terrain;

    private void Awake()
    {

        terrain = GetComponent<Terrain>();
    }

    public void InitalizeTerrainGenerator(int generatedSeed, int i, int j, int mapWitdth, int mapHeight)
    {
        seed = generatedSeed;
        xChunkOffset = i;
        yChunkOffset = j;

        width = mapWitdth;
        height = mapHeight;
        
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }
    
    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, PerlinNoise.GenerateMap(width, height, scale, seed, octaves, lucunarity, persistance));

        return terrainData;
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
}