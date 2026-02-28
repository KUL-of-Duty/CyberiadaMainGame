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
        var weapon = Instantiate(GunSO.weaponModel, transform.position + Vector3.up * 2, Quaternion.identity);
        weapon.transform.SetParent(transform, true);
        modelIndex = GunSO.weaponIndex;
    }
}
