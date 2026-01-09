using UnityEngine;
using Unity.Netcode;
using Newtonsoft.Json.Bson;
using NUnit.Framework;
using System.Collections.Generic;

public class EndlessTerrainGeneration : NetworkBehaviour
{
    [SerializeField] float maxRenderDistance;
    [SerializeField] Terrain levelTile;
    private GameObject player;
    public static Vector2Int playerChunkPos;

    
    public enum LevelTiles
    {
        levelTile = 1
    }

    int chunkSize;
    int chunkRenderDistance;
    int negChunkRenDist;
    [SerializeField] int mapWidth = 512;
    public bool activated = false;

    public NetworkList<Vector2Int> NVterrainList = new NetworkList<Vector2Int>();
    Dictionary<Vector2Int, GameObject> TerrainDict = new Dictionary<Vector2Int, GameObject>();
    List<Vector2Int> ActiveChunks = new List<Vector2Int>();


    public override void OnNetworkSpawn(){   
        base.OnNetworkSpawn();

        chunkSize = mapWidth;
        chunkRenderDistance = Mathf.RoundToInt(maxRenderDistance / chunkSize);
        negChunkRenDist = -Mathf.RoundToInt(maxRenderDistance / chunkSize) - 1;
    }
     
	void Update() {
        if (!activated || player == null) return;
            playerChunkPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.z);
            CreateVisibleChunks();
	}

    void CreateVisibleChunks()
    {
        int currentChunkCoordX =  Mathf.RoundToInt(player.transform.position.x / chunkSize);
        int currentChunkCoordY =  Mathf.RoundToInt(player.transform.position.z / chunkSize);

        if (Time.frameCount % 30 != 0) return;

        foreach (Vector2Int chunk in ActiveChunks)
        {
            TerrainDict[chunk].SetActive(false);
        }
        ActiveChunks.Clear();

        for (int yOffset = negChunkRenDist; yOffset <= chunkRenderDistance; yOffset++)
            for(int xOffset = negChunkRenDist; xOffset <= chunkRenderDistance; xOffset++)
            {
                Vector2Int viewedChunk = new Vector2Int(currentChunkCoordX + xOffset,currentChunkCoordY + yOffset);

                if (NVterrainList.Contains(viewedChunk) && TerrainDict.ContainsKey(viewedChunk))
                {
                    TerrainDict[viewedChunk].SetActive(true);
                    ActiveChunks.Add(viewedChunk);
                } 
                else
                {
                    CheckChunkServerRpc(viewedChunk, chunkSize);
                }
            }
    }

    public void AssignPlayer()
    {
        player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject;
    }

    public void LocalGenerateChunk(Vector2Int coord, int size)
    {
            Vector2Int position = coord * size;
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

            Terrain terrain = Instantiate(levelTile, positionV3, Quaternion.identity, gameObject.transform);
            TerrainDict.Add(coord, terrain.gameObject);
            ActiveChunks.Add(coord);
    }

    [Rpc(SendTo.ClientsAndHost, InvokePermission = RpcInvokePermission.Everyone)]
    public void InstantiateChunkGenerationServerRPC(Vector2Int coord, int size)
    {
        LocalGenerateChunk(coord, size);
        AddChunkToListServerRPC(coord);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void CheckChunkServerRpc(Vector2Int coord, int size)
    {
        if (NVterrainList.Contains(coord)) return;
        InstantiateChunkGenerationServerRPC(coord, size);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void AddChunkToListServerRPC(Vector2Int nvVec)
    {
        NVterrainList.Add(nvVec);
    }
}

 