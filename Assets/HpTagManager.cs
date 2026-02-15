using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;

public class HpTagManager : NetworkBehaviour
{
    public Text hpBarTag;
    public PlayerHpSystem playerHpSystem;

    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        hpBarTag.text = NetworkObjectId.ToString()+": "+playerHpSystem.targetHp.health.Value.ToString();
        Debug.Log(hpBarTag.text);
        playerHpSystem.targetHp.health.OnValueChanged+=HpBarTagUpdate;
    }
    // void Awake()
    // {
    //     if(!IsClient) return;
    //     hpBarTag.text = NetworkObjectId.ToString()+": "+playerHpSystem.targetHp.health.Value.ToString();
    //     Debug.Log(hpBarTag.text);
    // }
    void HpBarTagUpdate(float oldValue,float newValue){
        hpBarTag.text = NetworkObjectId.ToString()+": "+playerHpSystem.targetHp.health.Value.ToString();
    }
}
