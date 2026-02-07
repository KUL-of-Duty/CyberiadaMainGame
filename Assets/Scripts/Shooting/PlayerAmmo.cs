using UnityEngine;
using Unity.Netcode;

public class PlayerAmmo : NetworkBehaviour {
    public NetworkVariable<int> Ammo = new NetworkVariable<int>(1,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    public override void OnNetworkSpawn(){
        Ammo.Value = GetComponent<GunScript>().gunObjectScript.ammo;
        if(!IsOwner){
            Ammo.OnValueChanged+=OnAmmoChanged;
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
    }

}
