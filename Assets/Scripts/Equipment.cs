using UnityEngine;
using System.Linq; 

public class InventorySystem : MonoBehaviour
{
    private const int MAX_SLOTS = 5;
    
    public GameObject[] slots = new GameObject[MAX_SLOTS];

    public int selectedSlotIndex = 0;

    private int itemsCount; 

    void Start()
    {
        itemsCount = 0; 
        
        SelectWeapon();
    }

    void Update()
    {
        itemsCount = slots.Count(item => item != null); 
        
        int previousSelectedSlot = selectedSlotIndex;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
        {
            selectedSlotIndex++;
            if (selectedSlotIndex >= MAX_SLOTS)
            {
                selectedSlotIndex = 0;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
        {
            selectedSlotIndex--;
            if (selectedSlotIndex < 0)
            {
                selectedSlotIndex = MAX_SLOTS - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlotIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && itemsCount >= 1) selectedSlotIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && itemsCount >= 2) selectedSlotIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && itemsCount >= 3) selectedSlotIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5) && itemsCount >= 4) selectedSlotIndex = 4;
        
        if (selectedSlotIndex >= itemsCount && itemsCount > 0)
        {
            selectedSlotIndex = itemsCount - 1;
        }

        if (previousSelectedSlot != selectedSlotIndex)
        {
             SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        foreach (GameObject item in slots)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
        
        if (selectedSlotIndex >= 0 && selectedSlotIndex < MAX_SLOTS)
        {
            GameObject selectedItem = slots[selectedSlotIndex];
            if (selectedItem != null)
            {
                selectedItem.SetActive(true);
            }
        }
    }
}