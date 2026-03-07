using Unity.Netcode;
using UnityEngine.UI;

public class HpTagManager : NetworkBehaviour
{
    public Text hpBarTag;
    public PlayerHpSystem playerHpSystem;

    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        hpBarTag.text = playerHpSystem.targetHp.health.Value.ToString();
        playerHpSystem.targetHp.health.OnValueChanged+=HpBarTagUpdate;
    }
    void HpBarTagUpdate(float oldValue,float newValue){
        hpBarTag.text = playerHpSystem.targetHp.health.Value.ToString();
    }
}
