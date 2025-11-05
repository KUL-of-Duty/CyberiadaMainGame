using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private ItemSO _borrowedItem;
    private void OnTriggerEnter(Collider other)
    {
        _borrowedItem = other.gameObject.GetComponent<ItemSpawnerScript>().itemContainer;

        MusicManager.instance.PlaySound();

        Destroy(other.gameObject);
    }
}
