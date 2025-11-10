using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyManager : NetworkBehaviour
{
    NetworkVariable<int> netPlayerCount = new NetworkVariable<int>(0);
    [SerializeField] TMP_Text PlayerCount;
    private List<ulong> connectedClients = new List<ulong>();
    [SerializeField] GameObject playerModel;
    
    private void Awake()
    {
        netPlayerCount.OnValueChanged += IncresePlayerCount;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        LobbyJoinedServerRPC();
    }
    private void IncresePlayerCount(int previousValue, int newValue)
    {
        PlayerCount.text = "Player count: " + newValue;
    }

    public void StartGame(int buildIndex)
    {
        if (!IsServer) return;
        OpenSceneServerRPC(buildIndex);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void LobbyJoinedServerRPC()
    {
        netPlayerCount.Value++;
    }

    [Rpc(SendTo.Everyone)]
    public void OpenSceneServerRPC(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!connectedClients.Contains(clientId))
            connectedClients.Add(clientId);
    }
    private void OnClientDisconnected(ulong clientId)
    {
        connectedClients.Remove(clientId);
    }
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        foreach (ulong clientId in connectedClients)
        {
            GameObject playerInstance = Instantiate(playerModel);
            playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

}
