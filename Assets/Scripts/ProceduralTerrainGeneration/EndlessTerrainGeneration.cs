using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;


public class EndlessTerrainGeneration : NetworkBehaviour
{
    [SerializeField] float maxRenderDistance;
    [SerializeField] GameLevel levelTile;
    private GameObject player;
    public static Vector2 playerChunkPos;

    int chunkSize;
    int chunkRenderDistance; 

    Dictionary<Vector2, TerrainChunk> terrainChunkDict = new Dictionary<Vector2, TerrainChunk>();


    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        chunkSize = GetComponent<GenerateMapArray>().mapGridWidth;
        chunkRenderDistance = Mathf.RoundToInt(maxRenderDistance / chunkSize);
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

        for(int yOffset = -chunkRenderDistance; yOffset <= chunkRenderDistance; yOffset++)
            for(int xOffset = -chunkRenderDistance; xOffset <= chunkRenderDistance; xOffset++)
            {
                Vector2 viewedChunk = new Vector2(currentChunkCoordX + xOffset,currentChunkCoordY + yOffset);

                if(terrainChunkDict.ContainsKey(viewedChunk)){

                } else {
                    terrainChunkDict.Add(viewedChunk, new TerrainChunk(viewedChunk, chunkSize, levelTile,transform));
                }
            }
            
    }
    public class TerrainChunk {
    Vector2 position;
		Bounds bounds;

		public TerrainChunk(Vector2 coord, int size, GameLevel levelTile, Transform parent) {
			position = coord * size;
			bounds = new Bounds(position,Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

            Terrain terrain = Instantiate(levelTile.terrain);
            terrain.transform.parent = parent;
		}

		public void UpdateTerrainChunk() {
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerChunkPos));
		}
 
    }
}

