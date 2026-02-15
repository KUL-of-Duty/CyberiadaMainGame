using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;

public class PlayerAmmo : NetworkBehaviour {
    public NetworkVariable<int> Ammo = new NetworkVariable<int>(1,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public AmmoScript ammoScript;
    
    public override void OnNetworkSpawn(){
        if(!IsOwner) return;
            Ammo.Value = GetComponent<GunScript>().gunObjectScript.maxAmmo;
        Ammo.OnValueChanged+=OnAmmoChanged;
    }
    [ServerRpc]
    public void ConsumeAmmoServerRpc(int amount){
        if(GetAmmo()>=amount){
            Ammo.Value-=amount;
        }
    }
    public int GetAmmo(){
        return Ammo.Value;
    }
    void OnAmmoChanged(int oldValue, int newValue){
        Debug.Log("New Ammo Value: "+newValue);
        ammoScript.OnAmmoAmountChange(newValue);
    }

}
