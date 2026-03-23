using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMushroomAmountUI : NetworkBehaviour
{
    public TextMeshProUGUI mushroomAmount;
    public MushroomAtlas mushroomAtlas;
    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        mushroomAmount.text = mushroomAtlas.getMushroomCounter().ToString();
    }

    public void OnMushroomAmountChange(int newValue)
    {
        mushroomAmount.text = newValue.ToString();
    }
}
