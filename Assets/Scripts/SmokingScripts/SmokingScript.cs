using Unity.Netcode;
using UnityEngine;

public class SmokingScript : NetworkBehaviour
{
    public Animator animator;
    void Awake()
    {
    }
    void Update()
    {
        if(!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.G))
        {
            SmokeServerRpc();
        }
    }

    [ServerRpc]
    void SmokeServerRpc()
    {
        SmokeClientRpc();
    }

    [ClientRpc]
    void SmokeClientRpc()
    {
        animator.SetTrigger("Smoke");
    }
}
