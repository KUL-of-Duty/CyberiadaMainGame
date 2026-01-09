using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Podpięcia Sceny")]
    // Pusty obiekt pod kamerą, gdzie będą pojawiać się bronie
    public Transform weaponSocket;      
    public Transform cameraTransform;   
    public HotbarUI hotbarUI;           

    // Tablica 4 slotów na bronie (0: Rifle, 1: Pistol, 2: Melee, 3: Utility)
    private PhysicalWeapon[] slots = new PhysicalWeapon[4]; 
    private int currentSlotIndex = 4; // Indeks 4 oznacza "puste ręce"

    private PlayerMovement movement;

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
            int direction = (scroll < 0) ? 1 : -1;
            int nextSlot = currentSlotIndex;
            for (int i = 0; i < 5; i++)
            {
                nextSlot = (int)Mathf.Repeat(nextSlot + direction, 5);
                // Zatrzymaj się tylko na dłoni (4) lub zajętym slocie
                if (nextSlot == 4 || slots[nextSlot] != null) { EquipSlot(nextSlot); break; }
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) TryPickUp();
        if (Input.GetKeyDown(KeyCode.G)) DropCurrentItem();
    }

    void TryPickUp()
    {
        RaycastHit hit;
        // Raycast ignoruje gracza, szuka broni w wybranym zasiegu
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3.5f, ~LayerMask.GetMask("Player")))
        {
            PhysicalWeapon weapon = hit.collider.GetComponent<PhysicalWeapon>();
            if (weapon != null)
            {
                // Konwertuje nazwę z Enum na numer indeksu (np. Pistol -> 1)
                int targetSlot = (int)weapon.weaponType; 

                if (slots[targetSlot] != null) return; // Jeśli slot zajęty, nic nie rób

                slots[targetSlot] = weapon;
                weapon.transform.SetParent(weaponSocket);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                weapon.SetPhysics(false); // Wyłącza fizykę podniesionej broni

                EquipSlot(targetSlot);
            }
        }
    }

    public void EquipSlot(int index)
    {
        currentSlotIndex = index;
        // Ukryj wszystkie modele broni
        for (int i = 0; i < 4; i++) if (slots[i] != null) slots[i].gameObject.SetActive(false);

        // Jeśli wybrano slot z bronią, pokaż model i wyłącz bonus prędkości
        if (index < 4 && slots[index] != null)
        {
            slots[index].gameObject.SetActive(true);
            movement.SetEmptyHandBonus(false);
        }
        else
        {
            // Jeśli wybrano pusty slot lub klawisz 5, aktywuj bonus dłoni
            movement.SetEmptyHandBonus(true);
            currentSlotIndex = 4;
        }

        // Powiadom skrypt UI o zmianie
        if (hotbarUI != null) hotbarUI.UpdateUI(currentSlotIndex, slots);
    }

    void DropCurrentItem()
    {
        if (currentSlotIndex < 4 && slots[currentSlotIndex] != null)
        {
            PhysicalWeapon w = slots[currentSlotIndex];
            w.transform.SetParent(null);
            w.SetPhysics(true); // Włącza fizykę z powrotem
            if(w.rb != null) w.rb.AddForce(cameraTransform.forward * 5f, ForceMode.Impulse);
            slots[currentSlotIndex] = null;
            EquipSlot(4); // Wróć do dłoni po wyrzuceniu
        }
    }
}