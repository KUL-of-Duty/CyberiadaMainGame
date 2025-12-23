using Unity.Netcode;
using UnityEngine;

public class Target : NetworkBehaviour
{
    public NetworkVariable<float> health;
    float dieTime;
    public override void OnNetworkSpawn()
    {
         health = new NetworkVariable<float>(20,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    }
    public void TakeDamage(float amount)
    {
        if(!IsServer) return;
        Debug.Log("hit");
        health.Value-=amount;
        if (health.Value <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dieTime = Time.time;
        NetworkObject.Despawn();
    }


    // [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
    // public void TakeDamageServerRpc(float amount)
    // {
    //     TakeDamage(amount);
    // }

    // [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
    // void DieServerRpc()
    // {
    //     Die();
    // }
}
