using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class EndlessTerrainGeneration : NetworkBehaviour
{
    [SerializeField] float maxRenderDistance;
    [SerializeField] GameLevel levelTile;
    private GameObject player;
    public static Vector2 playerChunkPos;
    
    public enum LevelTiles
    {
        levelTile = 1
    }

    int chunkSize;
    int chunkRenderDistance;
    int negChunkRenDist;

    NetworkList<Vector2> NVterrainList = new NetworkList<Vector2>();


    public override void OnNetworkSpawn(){   
        base.OnNetworkSpawn();

        chunkSize = GetComponent<GenerateMapArray>().mapGridWidth;
        chunkRenderDistance = Mathf.RoundToInt(maxRenderDistance / chunkSize);
        negChunkRenDist = -Mathf.RoundToInt(maxRenderDistance / chunkSize) - 1;
    }

	void Update() {
        if(player == null){
            player = GameObject.FindWithTag("Player");
            return;
        }
		playerChunkPos = new Vector2 (player.transform.position.x, player.transform.position.z);
		UpdateVisibleChunks ();
	}

    void UpdateVisibleChunks()
    {
        int currentChunkCoordX =  Mathf.RoundToInt(player.transform.position.x / chunkSize);
        int currentChunkCoordY =  Mathf.RoundToInt(player.transform.position.z / chunkSize);

        for(int yOffset = negChunkRenDist; yOffset <= chunkRenderDistance; yOffset++)
            for(int xOffset = negChunkRenDist; xOffset <= chunkRenderDistance; xOffset++)
            { 
                Vector2 viewedChunk = new Vector2(currentChunkCoordX + xOffset,currentChunkCoordY + yOffset);
                if(NVterrainList.Contains(viewedChunk)){

                } else {
                    NVterrainList.Add(viewedChunk);
                    InstantiateChunkGenerationClientRPC(viewedChunk, chunkSize);
                }
            }
            
    }

    public void LocalGenerateChunk(Vector2 coord, int size)
    {
            Vector2 position = coord * size;
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

            Terrain terrain = Instantiate(levelTile.terrain, positionV3, Quaternion.identity, gameObject.transform);
    }

    [ClientRpc]
    public void InstantiateChunkGenerationClientRPC(Vector2 coord, int size)
    {
        LocalGenerateChunk(coord, size);
    }
}

 