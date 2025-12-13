using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject spawnLocation;
    private void Start()
    {
        if (!IsServer) return;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        GameObject playerSpawned = Instantiate(playerPrefab, spawnLocation.transform);
        playerSpawned.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
    }

    private void OnClientConnected(ulong player)
    {
        Debug.Log("XD");
        GameObject playerSpawned = Instantiate(playerPrefab, spawnLocation.transform);
        playerSpawned.GetComponent<NetworkObject>().SpawnAsPlayerObject(player);
    }
}   
