using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Podpięcia Sceny")]
    public Transform weaponSocket;      
    public Transform cameraTransform;   
    public HotbarUI hotbarUI;           

    [Header("Ekwipunek")]
    // 0: Rifle, 1: Pistol, 2: Melee, 3: Utility, 4: Puste ręce
    public GunObjectScript[] slots = new GunObjectScript[5];
    private GunObjectScript currentGun;
    [SerializeField] GameObject[] AllGuns;
    [SerializeField] GameObject ItemSpawnerPrefab;
    private int currentSlotIndex; 

    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        EquipSlot(4); // Zacznij z pustymi rękami
    }

    void Update()
    {
        HandleInput();
    }
     
    void HandleInput()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) EquipSlot(i);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int direction = (scroll < 0) ? 1 : -1;
            int nextSlot = (int)Mathf.Repeat(currentSlotIndex + direction, 5);
            EquipSlot(nextSlot);
        }

        if (Input.GetKeyDown(KeyCode.E)) TryPickUp();
        if (Input.GetKeyDown(KeyCode.G)) DropSlot(currentSlotIndex);
    }

    void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3.5f, ~LayerMask.GetMask("Player")))
        {
            if (hit.collider.gameObject.CompareTag("Item"))
            {
                ItemSpawner spawner = hit.collider.gameObject.GetComponentInParent<ItemSpawner>();
                if (spawner == null) return;

                GunObjectScript weapon = spawner.GunSO;
                int targetSlot = (int)weapon.weaponType; 

                if (slots[targetSlot] != null) DropSlot(targetSlot);

                slots[targetSlot] = weapon;
                spawner.WeaponDespawned();
                
                SoundManager.PlaySound(SoundType.PICKUP);
                EquipSlot(targetSlot);
            }
        }
    }

    public void EquipSlot(int index)
    {
        // Blokada przełączania na pusty slot (z wyjątkiem slotu 4)
        if (index != 4 && slots[index] == null) return;
        
        // Wyłączamy model obecnie trzymanej broni (jeśli istnieje)
        if (currentGun != null) 
            AllGuns[currentGun.weaponIndex].SetActive(false);

        currentSlotIndex = index;
        currentGun = slots[index];

        // Włączamy model nowej broni tylko jeśli slot nie jest pusty
        if (currentGun != null)
        {
            AllGuns[currentGun.weaponIndex].SetActive(true);
            movement.SetEmptyHandBonus(false);
        }
        else
        {
            // Bonus prędkości dla "pustych rąk" bez włączania modelu AllGuns[4]
            movement.SetEmptyHandBonus(true);
        }

        SoundManager.PlaySound(SoundType.WEAPONCHANGE);

        if (hotbarUI != null) hotbarUI.UpdateUI(currentSlotIndex, slots);
    }

    void DropSlot(int index)
    {
        if (index == 4 || slots[index] == null) return;

        GameObject droppedItem = Instantiate(ItemSpawnerPrefab, transform.position + transform.forward, Quaternion.identity);
        ItemSpawner spawner = droppedItem.GetComponent<ItemSpawner>();
        spawner.GunSO = slots[index];
        spawner.SpawnAfterStart();

        SoundManager.PlaySound(SoundType.DROP);

        slots[index] = null;
        EquipSlot(4);
    }
}