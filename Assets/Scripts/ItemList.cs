using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemList : NetworkBehaviour
{
    public static ItemList Instance;
    public GunObjectScript[] Items;
    public List<GameObject> SpawnedItemsList = new List<GameObject>();
    [SerializeField]
    GameObject ItemSpawnerPrefab;

    public override void OnNetworkSpawn()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [Rpc(SendTo.Everyone)]
    public void HideItemRPC(int spawnedIndex)
    {
        SpawnedItemsList[spawnedIndex].SetActive(false);
    }

    [Rpc(SendTo.Everyone)]
    public void ShowItemRPC(int itemIndex, ulong ownerID)
    {
        Instantiate(Items[itemIndex]);
    }

    [Rpc(SendTo.Everyone)]
    public void SpawnItemRPC(int itemIndex, Vector3 location)
    {
        var spawner = Instantiate(ItemSpawnerPrefab, transform.position, Quaternion.identity);
        var itemSpawner = spawner.GetComponent<ItemSpawner>();
        itemSpawner.GunSO = Items[itemIndex];
        itemSpawner.SpawnAfterStart();
    }

    [Rpc(SendTo.Everyone)]
    public void GiveItemRPC(int itemIndex, ulong ownerID)
    {
        Instantiate(Items[itemIndex], NetworkManager.ConnectedClients[ownerID].PlayerObject.gameObject.transform.position + Vector3.up * 2, Quaternion.identity);
    }

    [Rpc(SendTo.Everyone)]
    public void DespawnItemRPC(int spawnedIndex)
    {
        Destroy(SpawnedItemsList[spawnedIndex]);
    }
}
