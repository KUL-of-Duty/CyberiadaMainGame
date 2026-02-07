using Unity.Netcode;
using UnityEngine;

public class CigaretteEvents : MonoBehaviour
{
    PlayerHpSystem playerHpSystem;
    void Start()
    {
        playerHpSystem = GetComponentInParent<PlayerHpSystem>();
    }
    public void OnSmokeFinished()
    {
        Debug.Log("Animacja Zakońćzcona");
        playerHpSystem.HpRegenerationServerRpc(20);
    }
}
