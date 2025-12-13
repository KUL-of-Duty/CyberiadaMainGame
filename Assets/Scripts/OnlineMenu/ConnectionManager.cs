using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectionManager : NetworkBehaviour
{
    NetworkManager netManager;
    UnityTransport unityTransport;
    NetworkManager networkManager;

    [SerializeField] TMP_InputField ifipAddress;
    [SerializeField] TMP_InputField ifPort;

    [SerializeField] GameObject lobby;

    private void Awake()
    {
        netManager = NetworkManager.Singleton;
        unityTransport = netManager.GetComponent<UnityTransport>();
        networkManager = netManager.GetComponent<NetworkManager>();
    }
    public void JoinLobby()
    {
        unityTransport.SetConnectionData(ifipAddress.text, Convert.ToUInt16(ifPort.text));
        netManager.StartClient();
        ShowLobby();
    }

    public void HostLobby()
    {
        networkManager.StartHost();
        ShowLobby();
    }

    public void ShowLobby()
    {
        netManager.GetComponent<NetworkManager>().SceneManager.LoadScene("HeliLobby", LoadSceneMode.Single);
    }
}
