using Unity.Netcode;
using UnityEngine.UI;

public class AmmoScript : NetworkBehaviour
{
    public Text ammoAmount;
    public PlayerAmmo playerAmmo;
    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        ammoAmount.text = playerAmmo.Ammo.Value.ToString();
    }

    public void OnAmmoAmountChange(int newValue)
    {
        ammoAmount.text = playerAmmo.Ammo.Value.ToString();
    }
}
