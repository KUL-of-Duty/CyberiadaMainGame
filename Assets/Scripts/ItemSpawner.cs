using UnityEngine;

public class ItemSpawner : MonoBehaviour    
{
    [SerializeField] 
    int modelIndex;
    public GunObjectScript GunSO;
    public int itemID;
    ItemList itemManager;
    [SerializeField] 
    bool SpawnItem = false;
    private void Awake()
    {
        itemManager = FindAnyObjectByType<ItemList>();
    }
    void Start()
    {
        if (GunSO == null || !SpawnItem) return;
        itemManager.SpawnedItemsList.Add(gameObject);
        itemID = itemManager.SpawnedItemsList.Count - 1;
        modelIndex = GunSO.weaponIndex;

        Instantiate(GunSO.weaponModel, transform.position, Quaternion.identity, transform);
    }

    public void SpawnAfterStart()
    {
        if(itemManager == null) itemManager = FindAnyObjectByType<ItemList>();

        var weapon = Instantiate(GunSO.weaponModel, transform.position, Quaternion.identity);
        weapon.transform.SetParent(transform, true);
        itemManager.SpawnedItemsList.Add(gameObject);
        itemID = itemManager.SpawnedItemsList.Count - 1;
        modelIndex = GunSO.weaponIndex;

        ItemList.SpawnItemRPC(modelIndex, transform.position);
    }

    public void WeaponDespawned()
    {
        itemManager.DespawnItemRPC(itemID);
    }
}
