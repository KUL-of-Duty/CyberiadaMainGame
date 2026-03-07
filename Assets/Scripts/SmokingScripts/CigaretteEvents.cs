using Unity.Netcode;
using UnityEngine;

public class CigaretteEvents : MonoBehaviour
{
    PlayerHpSystem playerHpSystem;
    float regenerationValue = 20;
    void Start()
    {
        playerHpSystem = GetComponentInParent<PlayerHpSystem>();
    }
    public void OnSmokeFinished()
    {
        Debug.Log("Animacja Zakońćzcona");
        playerHpSystem.HpRegenerationServerRpc(regenerationValue);
    }
}
