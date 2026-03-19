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

    [SerializeField] TMP_InputField ifipAddress;
    [SerializeField] TMP_InputField ifPort;

    private void Awake()
    {
        netManager = NetworkManager.Singleton;
    }

    private void Update()
    {
        if(netManager == null) netManager = NetworkManager.Singleton;
    }
    public void JoinLobby()
    {
        netManager.GetComponent<UnityTransport>().SetConnectionData(ifipAddress.text, Convert.ToUInt16(ifPort.text));
        netManager.StartClient();
        SceneManager.LoadScene("MainLevel", LoadSceneMode.Single);
    }

    public void HostLobby()
    {
        netManager.GetComponent<UnityTransport>().SetConnectionData(ifipAddress.text, Convert.ToUInt16(ifPort.text));
        netManager.StartHost();
        ShowLobby();
    }

    public void ShowLobby()
    {
        netManager.GetComponent<NetworkManager>().SceneManager.LoadScene("MainLevel", LoadSceneMode.Single);
    }
}
