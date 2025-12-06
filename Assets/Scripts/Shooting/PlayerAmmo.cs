using UnityEngine;
using Unity.Netcode;

public class PlayerAmmo : NetworkBehaviour {
    public NetworkVariable<int> Ammo;

    public override void OnNetworkSpawn()
    {
        Ammo = new NetworkVariable<int>(GetComponentInChildren<gunScript>().gunObjectScript.ammo,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
        if(!IsOwner){
            Ammo.OnValueChanged+=(oldValue, newValue)=>{

                Debug.Log("Ammo: "+newValue);
            };
        }
    }

    [ServerRpc]
    public void ConsumeAmmoServerRpc(int amount){
        if(Ammo.Value>=amount)
            Ammo.Value-=amount;
    }

}
