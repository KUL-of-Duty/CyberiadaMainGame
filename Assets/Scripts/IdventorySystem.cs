using UnityEngine;
using System.Linq;

public class InventorySystem : MonoBehaviour
{
    private const int MAX_SLOTS = 5;
    private const int MAX_ITEM_SLOTS = 4;
    
    public GameObject[] itemSlots = new GameObject[MAX_ITEM_SLOTS];
    public int selectedSlotIndex = 0; 
    
    private int itemsCount; 
    private int totalAvailableSlots; 
    
    private Player playerMovement; 

    void Start()
    {
        playerMovement = GetComponent<Player>();
        if (playerMovement == null)
        {
            Debug.LogError("Brak skryptu Player na tym samym obiekcie, nie będzie można przyspieszyć gracza!");
        }
        
        itemsCount = itemSlots.Count(item => item != null);
        UpdateTotalAvailableSlots();
        
        if (selectedSlotIndex >= totalAvailableSlots)
        {
            selectedSlotIndex = totalAvailableSlots > 0 ? totalAvailableSlots - 1 : 0;
        }

        SelectSlot();
    }

    void Update()
    {
        // Ta linia jest kluczowa po podniesieniu/wyrzuceniu
        itemsCount = itemSlots.Count(item => item != null);
        UpdateTotalAvailableSlots(); 
        
        int previousSelectedSlot = selectedSlotIndex;

        // --- Obsługa Scrolla Myszki ---
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
        {
            selectedSlotIndex++;
            if (selectedSlotIndex >= totalAvailableSlots)
            {
                selectedSlotIndex = 0;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
        {
            selectedSlotIndex--;
            if (selectedSlotIndex < 0)
            {
                selectedSlotIndex = totalAvailableSlots > 0 ? totalAvailableSlots - 1 : 0;
            }
        }

        // --- Obsługa Klawiszy 1-5 ---
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i < totalAvailableSlots)
                {
                    selectedSlotIndex = i;
                }
                break;
            }
        }

        // --- Korekta wyboru po dropie, jeśli wybrano slot poza zakresem ---
        if (selectedSlotIndex >= totalAvailableSlots && totalAvailableSlots > 0)
        {
            selectedSlotIndex = totalAvailableSlots - 1;
        }
        
        if (previousSelectedSlot != selectedSlotIndex)
        {
             SelectSlot();
        }
    }
    
    void UpdateTotalAvailableSlots()
    {
        // Jeśli mamy 4 przedmioty, mamy 5 slotów (1-4 item, 5 pusta ręka)
        if (itemsCount == MAX_ITEM_SLOTS)
        {
            totalAvailableSlots = MAX_SLOTS; 
        }
        // Jeśli mamy 1-3 przedmiotów, mamy itemsCount + 1 slot (itemy + pusta ręka)
        else if (itemsCount > 0)
        {
            totalAvailableSlots = itemsCount + 1;
        }
        // Jeśli mamy 0 przedmiotów, nie mamy żadnych aktywnych slotów do przełączania
        else
        {
            totalAvailableSlots = 0; 
        }
    }

    void SelectSlot()
    {
        // 1. Dezaktywuj wszystkie przedmioty
        foreach (GameObject item in itemSlots)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
        
        // Domyślna dezaktywacja bonusu prędkości
        if (playerMovement != null)
        {
            playerMovement.SetSpeedBonus(false);
        }

        // 2. Obsługa Pustej Ręki (Slot itemsCount)
        // Jeśli wybrano indeks itemsCount (pierwszy wolny za przedmiotami)
        if (selectedSlotIndex == itemsCount && selectedSlotIndex < MAX_SLOTS && itemsCount > 0)
        {
            Debug.Log("Wybrano Pustą Rękę. Prędkość zwiększona.");
            if (playerMovement != null)
            {
                playerMovement.SetSpeedBonus(true);
            }
        }
        // 3. Obsługa slota z Przedmiotem
        else if (selectedSlotIndex >= 0 && selectedSlotIndex < itemsCount)
        {
            GameObject selectedItem = itemSlots[selectedSlotIndex];
            if (selectedItem != null)
            {
                selectedItem.SetActive(true);
                Debug.Log($"Wybrano slot: {selectedSlotIndex + 1}. Aktywowano przedmiot: {selectedItem.name}");
            }
        }
        else
        {
            Debug.Log("Nie wybrano aktywnego slota lub brak przedmiotów w ekwipunku.");
        }
    }

    // --- METODA PODNOSZENIA ---
    public bool AddItem(GameObject itemPrefab)
    {
        if (itemsCount < MAX_ITEM_SLOTS)
        {
            int firstFreeSlotIndex = itemsCount; 
            
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
            newItem.name = itemPrefab.name;
            
            // Konieczne wyłączenie kolizji i ustawienie go jako kinematic (dopóki jest w ekwipunku)
            Rigidbody rb = newItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            Collider coll = newItem.GetComponent<Collider>();
            if (coll != null)
            {
                coll.enabled = false;
            }

            itemSlots[firstFreeSlotIndex] = newItem;
            
            itemsCount++;
            UpdateTotalAvailableSlots(); 
            
            selectedSlotIndex = firstFreeSlotIndex;
            SelectSlot();

            Debug.Log($"Podniesiono przedmiot: {newItem.name} do slotu {firstFreeSlotIndex + 1}.");
            return true;
        }
        else
        {
            Debug.Log("Ekwipunek jest pełny (4 sloty zajęte). Nie można podnieść przedmiotu.");
            return false;
        }
    }
    
    // --- METODA WYRZUCANIA (NOWA) ---
    // Zwraca wyrzucony przedmiot, aby skrypt PickUp_n_Drop mógł go użyć
    public GameObject DropSelectedItem()
    {
        // Sprawdź, czy wybrany slot to slot z przedmiotem (nie Pusta Ręka)
        if (selectedSlotIndex >= 0 && selectedSlotIndex < itemsCount)
        {
            GameObject itemToDrop = itemSlots[selectedSlotIndex];

            if (itemToDrop != null)
            {
                // 1. Usuń przedmiot z tablicy slotów
                itemSlots[selectedSlotIndex] = null;

                // 2. Przesuń pozostałe przedmioty, by zachować ciągłość
                for (int i = selectedSlotIndex; i < MAX_ITEM_SLOTS - 1; i++)
                {
                    itemSlots[i] = itemSlots[i + 1];
                }
                itemSlots[MAX_ITEM_SLOTS - 1] = null; // Ostatni slot jest zawsze pusty po przesunięciu
                
                // 3. Zaktualizuj stan
                itemsCount = itemSlots.Count(item => item != null);
                UpdateTotalAvailableSlots(); 
                
                // 4. Zmień wybór na bezpieczny (np. Pustą Rękę, jeśli jest dostępna)
                if (totalAvailableSlots > 0)
                {
                    selectedSlotIndex = itemsCount; // Pusta ręka
                }
                else
                {
                    selectedSlotIndex = 0; // Domyślne 0, gdy ekwipunek pusty
                }
                
                SelectSlot();
                
                Debug.Log($"Wyrzucono przedmiot: {itemToDrop.name}.");
                return itemToDrop;
            }
        }
        Debug.Log("Nie można wyrzucić Pustej Ręki lub slot jest pusty.");
        return null;
    }
}