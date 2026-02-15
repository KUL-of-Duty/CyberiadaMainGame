using Unity.Netcode;
using UnityEngine;

public class Target : NetworkBehaviour{
    public float maxHealth = 100;
    public NetworkVariable<float> health=new NetworkVariable<float>(10,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public HPBarUI hpBarUI;
    float dieTime;

    public override void OnNetworkSpawn(){
         if(IsServer) health.Value = maxHealth;
         health.OnValueChanged += OnHealthChanged;
    }
    public override void OnNetworkDespawn(){
        health.OnValueChanged -= OnHealthChanged;
    }
    
    public void TakeDamage(float amount){
        TakeDamageServerRpc(amount);
    }

    void Die(){
        dieTime = Time.time;
        NetworkObject.Despawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float amount){
        health.Value-=amount;
        if (health.Value <= 0)
        {
            Die();
        }
    }

    void OnHealthChanged(float oldValue, float newValue){
        Debug.Log("HP updated: " + newValue);
        if (!IsOwner) return;
        hpBarUI?.OnSetHealth(newValue);
    }
}
