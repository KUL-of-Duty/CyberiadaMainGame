// PickUp_n_Drop.cs
using UnityEngine;

public class PickUp_n_Drop : MonoBehaviour
{
    [SerializeField]
    private float pickupRange = 3f;
    [SerializeField]
    private float dropForce = 5f;

    private InventorySystem inventory;
    private Camera playerCam;
    
    // NOWA ZMIENNA: Potrzebna, aby wiedzieć, gdzie upuścić.
    private Transform dropPoint; 

    void Start()
    {
        inventory = GetComponent<InventorySystem>();
        playerCam = Camera.main;
        
        if (inventory == null || playerCam == null)
        {
            Debug.LogError("Brak wymaganego komponentu InventorySystem na tym obiekcie lub brak kamery z tagiem 'MainCamera'.");
            enabled = false;
            return;
        }
        
        // Sprawdzenie, czy jest punkt do trzymania przedmiotów.
        // Jeśli go nie ma, użyj pozycji gracza.
        dropPoint = inventory.itemHoldPoint != null ? inventory.itemHoldPoint.parent : transform;
        if (dropPoint == null) dropPoint = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUp();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TryDrop();
        }
    }

    void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickupItem"))
            {
                GameObject itemToPickUp = hit.collider.gameObject;
                ItemData itemData = itemToPickUp.GetComponent<ItemData>(); // POBIERZ TYP

                if (itemData != null)
                {
                    // Używamy oryginalnego obiektu jako "prefabu" do utworzenia instancji i przekazujemy typ
                    bool pickedUp = inventory.AddItem(itemToPickUp, itemData.itemType);

                    if (pickedUp)
                    {
                        // USUŃ ORYGINALNY OBIEKT ZE ŚWIATA
                        Destroy(itemToPickUp); 
                    }
                }
                else
                {
                    Debug.LogError("Obiekt z tagiem 'PickupItem' nie posiada komponentu ItemData!");
                }
            }
        }
    }
    
    void TryDrop()
    {
        GameObject itemToDrop = inventory.DropSelectedItem();

        if (itemToDrop != null)
        {
            itemToDrop.transform.parent = null;
            
            // Ustaw pozycję przed graczem (używając dropPoint lub kamery)
            Vector3 dropPosition = playerCam.transform.position + playerCam.transform.forward;
            itemToDrop.transform.position = dropPosition;

            Rigidbody rb = itemToDrop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(playerCam.transform.forward * dropForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * dropForce * 0.5f, ForceMode.Impulse);
            }
            
            Collider coll = itemToDrop.GetComponent<Collider>();
            if (coll != null)
            {
                coll.enabled = true;
            }
            
            // ItemToDrop jest już aktywny w SelectSlot, ale dla pewności zostawiamy
            itemToDrop.SetActive(true); 
        }
    }
}