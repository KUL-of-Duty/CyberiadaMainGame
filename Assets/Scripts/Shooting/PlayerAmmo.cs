using UnityEngine;
using Unity.Netcode;

public class PlayerAmmo : NetworkBehaviour {
    public NetworkVariable<int> Ammo = new NetworkVariable<int>(1,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public AmmoScriptUI ammoScript;
    
    public override void OnNetworkSpawn(){
        Ammo.OnValueChanged += OnAmmoChanged;
        OnAmmoChanged(Ammo.Value, Ammo.Value);
        if (IsServer)
        {
            Ammo.Value = GetComponent<GunScript>().gunObjectScript.maxAmmo;
        }
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
