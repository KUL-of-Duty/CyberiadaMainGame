using UnityEngine;
//bron musi miec tag PickupItem
public class PickUp_n_Drop : MonoBehaviour
{
    [SerializeField]
    private float pickupRange = 3f;
    [SerializeField]
    private float dropForce = 5f;

    private InventorySystem inventory;
    private Camera playerCam;

    void Start()
    {
        inventory = GetComponent<InventorySystem>();
        playerCam = Camera.main;
        
        if (inventory == null || playerCam == null)
        {
            Debug.LogError("Brak wymaganego komponentu InventorySystem na tym obiekcie lub brak kamery z tagiem 'MainCamera'.");
            enabled = false;
        }
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
                
                // Używamy oryginalnego obiektu jako "prefabu" do utworzenia instancji
                bool pickedUp = inventory.AddItem(itemToPickUp);

                if (pickedUp)
                {
                    Destroy(itemToPickUp);
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
            
            // Ustaw pozycję przed graczem
            itemToDrop.transform.position = playerCam.transform.position + playerCam.transform.forward;

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
            
            itemToDrop.SetActive(true);
        }
    }
}