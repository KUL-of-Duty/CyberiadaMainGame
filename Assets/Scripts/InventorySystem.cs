using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Podpięcia Sceny")]
    // Pusty obiekt pod kamerą, gdzie będą pojawiać się bronie
    public Transform weaponSocket;      
    public Transform cameraTransform;   
    public HotbarUI hotbarUI;           

    // Tablica 4 slotów na bronie (0: Rifle, 1: Pistol, 2: Melee, 3: Utility)
    public GunObjectScript[] slots = new GunObjectScript[5];
    private GunObjectScript currentGun;
    [SerializeField] GameObject[] AllGuns;
    [SerializeField] GameObject ItemSpawnerPrefab;
    private int currentSlotIndex; // Indeks 4 oznacza "puste ręce"

    private PlayerMovement movement;
    ItemList itemManager;

    private void Awake()
    {
        itemManager = FindAnyObjectByType<ItemList>();
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        EquipSlot(4); // Zacznij z pustymi rękami
    }

    void Update()
    {
        HandleInput(); // Obsługa klawiszy i scrolla
    }
     
    void HandleInput()
    {
        // Wybór klawiszami 1-5
        for (int i = 0; i < 5; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) EquipSlot(i);

        // Scrollowanie z przeskakiwaniem pustych slotów
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int direction = (scroll < 0) ? 1 : -1; // Odwrócony scroll (Dół = następny)
            int nextSlot = currentSlotIndex;
            for (int i = 0; i < 5; i++)
            {
                nextSlot = (int)Mathf.Repeat(nextSlot + direction, 5);
                // Zatrzymaj się tylko na dłoni (4) lub zajętym slocie
                if (nextSlot == 4 || slots[nextSlot] != null) { EquipSlot(nextSlot); break; }
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) TryPickUp();
        if (Input.GetKeyDown(KeyCode.G)) DropSlot(currentSlotIndex);
    }

    void TryPickUp()
    {
        RaycastHit hit;
        // Raycast ignoruje gracza, szuka broni w zasięgu 3.5m
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3.5f, ~LayerMask.GetMask("Player")))
        {
            if (!hit.collider.gameObject.CompareTag("Item")) return;
            GunObjectScript weapon = hit.collider.gameObject.GetComponentInParent<ItemSpawner>().GunSO;
            // Konwertuje nazwę z Enum na numer indeksu (np. Pistol -> 1)
            int targetSlot = (int)weapon.weaponType; 

            // --- MAKS --- Wprowadziłem wymiane konkretnego slota, na ten który chcemy wziąść
            if (slots[targetSlot] != null)
            {
                EquipSlot(targetSlot);
                DropSlot(targetSlot);
            }
            hit.collider.gameObject.GetComponentInParent<ItemSpawner>().WeaponDespawned();
            slots[targetSlot] = weapon;
            EquipSlot(targetSlot);
        }
    }

    public void EquipSlot(int index)
    {
        // --- MAKS --- Wymieniłem chowanie każdego obieku, na chowanie aktualnie trzymanego
        if(currentSlotIndex == index || slots[index] == null) return;
        if (currentGun != null) AllGuns[currentGun.weaponIndex].SetActive(false);
        currentGun = slots[index];
        currentSlotIndex = index;
        // Jeśli wybrano slot z bronią, pokaż model i wyłącz bonus prędkości
        if (index < 4)
        {
            AllGuns[currentGun.weaponIndex].SetActive(true);
            movement.SetEmptyHandBonus(false);
        }
        else
        {
            AllGuns[currentGun.weaponIndex].SetActive(true);
            movement.SetEmptyHandBonus(true);
        }

        // Powiadom skrypt UI o zmianie
        if (hotbarUI != null) hotbarUI.UpdateUI(currentSlotIndex, slots);
    }

    void DropSlot(int index)
    {
        if (currentSlotIndex == 4) return;

        if (currentSlotIndex < 4 && slots[currentSlotIndex] != null)
        {
            var spawner = Instantiate(ItemSpawnerPrefab, transform.position, Quaternion.identity);
            var itemSpawner = spawner.GetComponent<ItemSpawner>();
            itemSpawner.GunSO = currentGun;
            itemSpawner.SpawnAfterStart();
            currentGun = null;
            slots[index] = null;
            EquipSlot(4); // Wróć do dłoni po wyrzuceniu
        }
    }
}