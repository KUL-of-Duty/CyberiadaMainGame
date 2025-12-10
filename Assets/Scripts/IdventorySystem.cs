using UnityEngine;
using System.Linq;
using System.Collections.Generic; // Dodaj do użycia słownika

public class InventorySystem : MonoBehaviour
{
    // 1. Zdefiniowanie stałych slotów na podstawie ItemType (bez EmptyHand)
    private const int NUM_EQUIPPABLE_SLOTS = 4; // Pistol, Rifle, Melee, Utility
    
    // Tablica przedmiotów mapowana na indeksy ItemType: 0-Pistol, 1-Rifle, 2-Melee, 3-Utility
    public GameObject[] itemSlots = new GameObject[NUM_EQUIPPABLE_SLOTS]; 
    
    // Aktualnie wybrany typ.
    public ItemType selectedItemType = ItemType.EmptyHand;
    
    // Dostęp do Player, aby ustawić bonus prędkości
    private Player playerMovement; 

    // Referencja do miejsca trzymania broni
    public Transform itemHoldPoint; 

    void Start()
    {
        playerMovement = GetComponent<Player>();
        if (playerMovement == null)
        {
            Debug.LogError("Brak skryptu Player na tym samym obiekcie, nie będzie można przyspieszyć gracza!");
        }
        
        if (itemHoldPoint == null)
        {
            Debug.LogError("itemHoldPoint nie jest ustawiony w InventorySystem!");
        }
        
        SelectSlot();
    }

    void Update()
    {
        ItemType previousSelectedType = selectedItemType;
        ItemType newSelectedType = selectedItemType;

        // --- Obsługa Scrolla Myszki ---
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        if (scrollInput != 0f)
        {
            // 1. Stwórz listę aktualnie zajętych slotów (typy ItemType) oraz EmptyHand
            List<ItemType> availableTypes = GetAvailableItemTypes();
            
            // Jeśli mamy tylko EmptyHand, nie rób nic
            if (availableTypes.Count <= 1) return;

            // 2. Znajdź obecny indeks w dostępnej liście
            int currentIndex = availableTypes.IndexOf(selectedItemType);
            
            if (currentIndex == -1) // Powinno się zdarzyć tylko w specyficznych, błędnych sytuacjach
            {
                currentIndex = 0;
            }

            // 3. Oblicz nowy indeks z cyklicznym przechodzeniem
            if (scrollInput > 0f) // Scroll w górę (następny)
            {
                currentIndex = (currentIndex + 1) % availableTypes.Count;
            }
            else if (scrollInput < 0f) // Scroll w dół (poprzedni)
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = availableTypes.Count - 1;
                }
            }

            newSelectedType = availableTypes[currentIndex];
        }
        
        // --- Obsługa Klawiszy 1-5 ---
        for (int i = 0; i < NUM_EQUIPPABLE_SLOTS + 1; i++) // 1-4 Itemy, 5 Pusta Ręka
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // Przełączanie za pomocą klawiszy jest prostsze, bo ma priorytet
                if (i < NUM_EQUIPPABLE_SLOTS && itemSlots[i] == null)
                {
                    // Jeśli slot itemu (0-3) jest pusty, przełącz na EmptyHand (indeks 4)
                    newSelectedType = ItemType.EmptyHand;
                }
                else
                {
                    // Wybierz typ odpowiadający klawiszowi
                    newSelectedType = (ItemType)i;
                }
                break;
            }
        }
        
        // Aktualizacja
        selectedItemType = newSelectedType;

        if (previousSelectedType != selectedItemType)
        {
            SelectSlot();
        }
    }
    
    // NOWA METODA: Tworzy listę aktualnie dostępnych (zajętych) ItemType, włączając EmptyHand.
    private List<ItemType> GetAvailableItemTypes()
    {
        List<ItemType> available = new List<ItemType>();
        
        // Dodaj wszystkie zajęte sloty
        for (int i = 0; i < NUM_EQUIPPABLE_SLOTS; i++)
        {
            if (itemSlots[i] != null)
            {
                // Indeksy 0, 1, 2, 3 odpowiadają ItemType.Pistol, ItemType.Rifle itd.
                available.Add((ItemType)i); 
            }
        }
        
        // Zawsze dodaj Pustą Rękę jako opcję (jej indeks to 4)
        available.Add(ItemType.EmptyHand);
        
        return available;
    }

    // Zwraca indeks w tablicy (0-3) dla danego typu ItemType
    private int GetItemSlotIndex(ItemType type)
    {
        // Sprawdzanie, czy to nie jest EmptyHand, które jest poza tablicą itemSlots
        if (type == ItemType.EmptyHand) return -1;
        
        return (int)type;
    }

    // Pozostałe metody (SelectSlot, AddItem, DropSelectedItem) pozostawiamy bez zmian,
    // ponieważ operują już na selectedItemType, a nie na indeksach scrolla.
    
    void SelectSlot()
    {
        // 1. Dezaktywuj wszystkie przedmioty i ustaw domyślny bonus prędkości na false
        foreach (GameObject item in itemSlots)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
        
        bool isHandEmpty = selectedItemType == ItemType.EmptyHand;
        
        if (playerMovement != null)
        {
            playerMovement.SetSpeedBonus(isHandEmpty);
        }
        
        // 2. Obsługa slota z Przedmiotem
        if (!isHandEmpty)
        {
            int slotIndex = GetItemSlotIndex(selectedItemType);
            
            if (slotIndex >= 0 && slotIndex < NUM_EQUIPPABLE_SLOTS)
            {
                GameObject selectedItem = itemSlots[slotIndex];
                
                if (selectedItem != null)
                {
                    // Ustaw pozycję, rotację i aktywuj
                    selectedItem.transform.SetParent(itemHoldPoint, false);
                    selectedItem.transform.localPosition = Vector3.zero;
                    selectedItem.transform.localRotation = Quaternion.identity;
                    
                    selectedItem.SetActive(true);
                    Debug.Log($"Wybrano slot: {selectedItemType}. Aktywowano przedmiot: {selectedItem.name}");
                    return; // Koniec, jeśli wybrano przedmiot
                }
            }
            
            // Jeśli selectedItemType nie jest EmptyHand, ale slot jest pusty
            // Automatycznie przełącz na Pustą Rękę, jeśli tak się zdarzy
            selectedItemType = ItemType.EmptyHand; 
            SelectSlot(); // Wywołaj ponownie, aby aktywować bonus prędkości
            return;
        }
        
        // 3. Obsługa Pustej Ręki
        Debug.Log("Wybrano Pustą Rękę. Prędkość ustawiona.");
    }


    // --- METODA PODNOSZENIA ---
    public bool AddItem(GameObject itemPrefab, ItemType type)
    {
        if (type == ItemType.EmptyHand)
        {
            Debug.LogError("Nie można podnieść przedmiotu typu EmptyHand.");
            return false;
        }
        
        int slotIndex = GetItemSlotIndex(type);
        
        if (itemSlots[slotIndex] == null)
        {
            // 1. Instancjacja i konfiguracja
            GameObject newItem = Instantiate(itemPrefab, itemHoldPoint.position, Quaternion.identity, itemHoldPoint);
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
            
            // 2. Dodaj do slotu
            itemSlots[slotIndex] = newItem;
            
            // 3. Ustaw nowy wybrany slot
            selectedItemType = type;
            SelectSlot();

            Debug.Log($"Podniesiono przedmiot: {newItem.name} do slotu {type}.");
            return true;
        }
        else
        {
            Debug.Log($"Slot {type} jest już zajęty przez: {itemSlots[slotIndex].name}. Nie można podnieść.");
            return false;
        }
    }
    
    public GameObject DropSelectedItem()
    {
        if (selectedItemType == ItemType.EmptyHand)
        {
            Debug.Log("Nie można wyrzucić Pustej Ręki.");
            return null;
        }
        
        int slotIndex = GetItemSlotIndex(selectedItemType);
        
        if (slotIndex >= 0 && slotIndex < NUM_EQUIPPABLE_SLOTS && itemSlots[slotIndex] != null)
        {
            GameObject itemToDrop = itemSlots[slotIndex];

            // 1. Usuń przedmiot z tablicy slotów
            itemSlots[slotIndex] = null;
            
            // 2. Ustaw nowy wybór (zawsze na Pustą Rękę po wyrzuceniu)
            selectedItemType = ItemType.EmptyHand; 
            SelectSlot();
            
            Debug.Log($"Wyrzucono przedmiot: {itemToDrop.name} ze slotu {slotIndex + 1}.");
            return itemToDrop;
        }
        
        Debug.Log("Wybrany slot jest pusty.");
        return null;
    }
}