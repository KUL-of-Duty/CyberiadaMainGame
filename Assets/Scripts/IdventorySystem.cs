// IdventorySystem.cs
using UnityEngine;
using System.Linq;
using System.Collections.Generic; // Dodaj do użycia słownika

public class InventorySystem : MonoBehaviour
{
    // 1. Zdefiniowanie stałych slotów na podstawie ItemType (bez EmptyHand)
    private const int NUM_EQUIPPABLE_SLOTS = 4; // Pistol, Rifle, Melee, Utility
    
    // Tablica przedmiotów mapowana na indeksy ItemType: 0-Pistol, 1-Rifle, 2-Melee, 3-Utility
    // Uwaga: Używamy GameObject, bo to będzie instancja przedmiotu trzymana w ręce.
    public GameObject[] itemSlots = new GameObject[NUM_EQUIPPABLE_SLOTS]; 
    
    // Aktualnie wybrany typ. Zaczynamy od Pustej Ręki.
    public ItemType selectedItemType = ItemType.EmptyHand;
    
    // Dostęp do Player, aby ustawić bonus prędkości
    private Player playerMovement; 

    // Referencja do miejsca trzymania broni (można ustawić w Inspekotorze, np. Child obiektu gracza)
    public Transform itemHoldPoint; 

    void Start()
    {
        playerMovement = GetComponent<Player>();
        if (playerMovement == null)
        {
            Debug.LogError("Brak skryptu Player na tym samym obiekcie, nie będzie można przyspieszyć gracza!");
        }
        
        // Upewnij się, że itemHoldPoint jest ustawiony!
        if (itemHoldPoint == null)
        {
             Debug.LogError("itemHoldPoint nie jest ustawiony w InventorySystem!");
        }
        
        SelectSlot();
    }

    void Update()
    {
        ItemType previousSelectedType = selectedItemType;

        // --- Obsługa Scrolla Myszki ---
        int currentTypeIndex = (int)selectedItemType;
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
        {
            currentTypeIndex = (currentTypeIndex + 1) % (NUM_EQUIPPABLE_SLOTS + 1); // +1 dla EmptyHand
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
        {
            currentTypeIndex--;
            if (currentTypeIndex < 0)
            {
                currentTypeIndex = NUM_EQUIPPABLE_SLOTS; // EmptyHand
            }
        }
        
        // --- Obsługa Klawiszy 1-5 ---
        for (int i = 0; i < NUM_EQUIPPABLE_SLOTS + 1; i++) // 1-4 Itemy, 5 Pusta Ręka
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentTypeIndex = i;
                break;
            }
        }

        // Zawsze upewniamy się, że nie wybrano slotu itemu, który jest pusty.
        // Jeśli slot 1 (Pistol) jest pusty, skok na 1/Alpha1 wybierze EmptyHand.
        if (currentTypeIndex < NUM_EQUIPPABLE_SLOTS && itemSlots[currentTypeIndex] == null)
        {
            // Przełącz na EmptyHand, jeśli wybrany slot jest pusty
             selectedItemType = ItemType.EmptyHand;
        }
        else
        {
             selectedItemType = (ItemType)currentTypeIndex;
        }
        
        if (previousSelectedType != selectedItemType)
        {
             SelectSlot();
        }
    }
    
    // Zwraca indeks w tablicy (0-3) dla danego typu ItemType
    private int GetItemSlotIndex(ItemType type)
    {
        return (int)type;
    }

    // Nowa implementacja SelectSlot
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
    
    // --- METODA WYRZUCANIA (NOWA) ---
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