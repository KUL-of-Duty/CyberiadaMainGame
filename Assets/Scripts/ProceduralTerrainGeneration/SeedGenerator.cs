using UnityEngine;
using Unity.Netcode;

public class SeedGenerator : NetworkBehaviour
{
    public NetworkVariable<int> ServerSeed = new NetworkVariable<int>(0);
    [SerializeField] int setSeed;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            ServerSeed.Value = Random.Range(1, 99999);
        }
    }
}
