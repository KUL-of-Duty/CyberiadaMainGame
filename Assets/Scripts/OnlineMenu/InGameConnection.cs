using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InGameConnection : NetworkBehaviour
{
    [SerializeField] GameObject LevelManager;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerCountText;
    //int playerCount = 1;

    private void Start()
    {
    }

    public void StartGame()
    {
        if(!IsServer) return;
        StartWorldGenerationClientRPC();
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject player = Instantiate(playerPrefab, GetSpawnPoint(), Quaternion.identity);

            player.GetComponent<NetworkObject>()
                  .SpawnAsPlayerObject(client.ClientId);
        }
        AssignPlayerClientRPC();
    }
    Vector3 GetSpawnPoint() 
    {
        return new Vector3(Random.Range(0, 25), 25, Random.Range(0, 25));
    }

    [ClientRpc]
    public void StartWorldGenerationClientRPC()
    {
        LevelManager.GetComponent<EndlessTerrainGeneration>().activated = true;
        gameObject.SetActive(false);
    }

    [ClientRpc]
    public void AssignPlayerClientRPC()
    {
        LevelManager.GetComponent<EndlessTerrainGeneration>().AssignPlayer();
    }
}
