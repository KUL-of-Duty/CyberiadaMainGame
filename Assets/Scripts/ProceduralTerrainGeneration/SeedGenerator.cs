using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class SeedGenerator : NetworkBehaviour
{
    public NetworkVariable<int> ServerSeed = new NetworkVariable<int>(0);
    public IEnumerator InitalizeSeedGeneration()
    {
        if (IsServer)
        {
            ServerSeed.Value = Random.Range(1, 99999);
            yield return new WaitUntil(() => ServerSeed.Value != 0);
        }
        else
        {
            yield return new WaitUntil(() => ServerSeed.Value != 0);
        }
    }

    public int GetSeed() 
    {
        return ServerSeed.Value;
    }
}
