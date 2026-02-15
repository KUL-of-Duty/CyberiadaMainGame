using UnityEngine;
using Unity.Netcode;

public class PlayerHpSystem : NetworkBehaviour
{
    public Target targetHp;
    void Awake()
    {
        targetHp = GetComponent<Target>();
    }
    public void HpRegeneration(float amount)
    {
        HpRegenerationServerRpc(amount);
    }

    [ServerRpc]
    public void HpRegenerationServerRpc(float amount)
    {
        if(targetHp == null) return;
        if(!IsServer||amount<=0) return;
        if(targetHp.health.Value<targetHp.maxHealth)
            targetHp.health.Value += amount;
        else targetHp.health.Value = targetHp.maxHealth;
        Debug.Log("HP regenerated, new vlaue: "+targetHp.health.Value);
    }
}
