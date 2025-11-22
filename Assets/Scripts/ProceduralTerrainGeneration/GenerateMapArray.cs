using Unity.Netcode;
using UnityEngine;

public class GenerateMapArray : NetworkBehaviour
{
    [SerializeField] GameLevel mapTerrain;
    public int mapGridWidth;
    public int mapGridHeight;
    [SerializeField] int chunkBorder;
    int _seed;
    System.Random _rng;
    bool _wasClicked = false;
    [Header("Chunks")]
    int[,] _chunkSeed;

    public void GetSeed()
    {
        _seed = gameObject.GetComponent<SeedGenerator>().ServerSeed.Value;
    }

    public void GenerateTerrain()
    {
        if (_wasClicked) return;
        if (IsOwner)
        {
            GetSeed();
            _rng = new System.Random(_seed);

            for (int i = 0; i < chunkBorder; i++)
                for (int j = 0; j < chunkBorder; j++)
                {
                    //_chunkSeed[i, j] = (int)_rng.NextDouble();
                    Terrain terrain = Instantiate(mapTerrain.terrain);
                    terrain.gameObject.GetComponent<TerrainGenerator>().InitalizeTerrainGenerator(_seed, i, j, mapGridWidth, mapGridHeight);
                    terrain.transform.position = GetLocation(i, j, mapGridHeight);
                }   
        }
        _wasClicked = true;
    }
        
    private Vector3 GetLocation(int i, int j, int x)
    {
        Vector3 pos = new Vector3(i * x, 0, j * x);
        return pos;
    }
}
