using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InGameConnection : NetworkBehaviour
{
    [SerializeField] GameObject LevelManager;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerCountText;

    private void Start()
    {
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientChanged;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientChanged;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientChanged;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientChanged;
    }

    private void OnClientChanged(ulong clientId)
    {
        UpdatePlayerCount();
    }

    public void UpdatePlayerCount()
    {
        int playerCount = NetworkManager.Singleton.ConnectedClientsList.Count;

        playerCountText.GetComponent<TMP_Text>().text = "Players: " + playerCount;
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
        Time.timeScale = 1.0f;
    }

    [ClientRpc]
    public void AssignPlayerClientRPC()
    {
        LevelManager.GetComponent<EndlessTerrainGeneration>().AssignPlayer();
    }
}
