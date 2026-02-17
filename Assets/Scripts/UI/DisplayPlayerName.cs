using Unity.Netcode;
using UnityEngine.UI;

public class DisplayPlayerName : NetworkBehaviour
{
    public Text PlayerName;
    public override void OnNetworkSpawn()
    {
        //if(!IsOwner) return;
        PlayerName.text = OwnerClientId.ToString();
    }
}
