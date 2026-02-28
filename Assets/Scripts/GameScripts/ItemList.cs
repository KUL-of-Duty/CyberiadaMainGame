using System.Collections.Generic;
using System.Linq;
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
    public void HideItemRPC(int itemIndex, ulong ownerID)
    {
        NetworkManager.ConnectedClients.TryGetValue(ownerID, out NetworkClient client);
        client.PlayerObject.GetComponent<InventorySystem>().AllGuns[itemIndex].SetActive(false);
    }

    [Rpc(SendTo.Everyone)]
    public void ShowItemRPC(int itemIndex, ulong ownerID)
    {
        NetworkManager.ConnectedClients.TryGetValue(ownerID, out NetworkClient client);
        client.PlayerObject.GetComponent<InventorySystem>().AllGuns[itemIndex].SetActive(true);
    }

    [Rpc(SendTo.Everyone)]
    public void SpawnItemRPC(int itemIndex, Vector3 location)
    {
        var spawner = Instantiate(ItemSpawnerPrefab, location, Quaternion.identity);
        var itemSpawner = spawner.GetComponent<ItemSpawner>();
        itemSpawner.GunSO = Items[itemIndex];
        itemSpawner.itemID = SpawnedItemsList.Count;
        SpawnedItemsList.Add(spawner);
        itemSpawner.SpawnAfterStart();
    }
    
    [Rpc(SendTo.Everyone)]
    public void GiveItemRPC(int itemIndex, Vector3 location)
    {
        var spawner = Instantiate(ItemSpawnerPrefab, location, Quaternion.identity);
        var itemSpawner = spawner.GetComponent<ItemSpawner>();
        itemSpawner.GunSO = Items[itemIndex];
        itemSpawner.itemID = SpawnedItemsList.Count;
        SpawnedItemsList.Add(spawner);
        itemSpawner.SpawnAfterStart();
    }

    [Rpc(SendTo.Everyone)]
    public void DespawnItemRPC(int spawnedIndex)
    {
        Destroy(SpawnedItemsList[spawnedIndex]);
    }
    
}
