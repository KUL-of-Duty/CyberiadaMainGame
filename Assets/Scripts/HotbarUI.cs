using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    [Header("Referencje UI")]
    // Przeciągnij tutaj obrazy tła 5 slotów (4 bronie + 1 dłoń)
    public Image[] slotBackgrounds;
    // Przeciągnij tutaj obrazy ikon dla 4 slotów broni
    public Image[] slotIcons;

    [Header("Kolory")]
    public Color activeColor = Color.white; // Kolor wybranego slotu
    public Color inactiveColor = new Color(0.2f, 0.2f, 0.2f, 0.5f); // Kolor nieaktywnych

    public void UpdateUI(int activeIndex, GunObjectScript[] inventorySlots)
    {
        // 1. Pętla zmienia kolor tła każdego slotu
        for (int i = 0; i < slotBackgrounds.Length; i++)
        {
            if (slotBackgrounds[i] != null)
            {
                // Jeśli i zgadza się z wybranym indeksem, ustaw kolor aktywny
                slotBackgrounds[i].color = (i == activeIndex) ? activeColor : inactiveColor;
            }
        }

        // 2. Aktualizacja ikon (tylko sloty 0-3, bo dłoń ma stałą ikonę lub brak)
        for (int i = 0; i < 4; i++)
        {
            if (inventorySlots[i] != null)
            {
                slotIcons[i].sprite = inventorySlots[i].weaponIcon;
                slotIcons[i].enabled = true;
            }
            else
            {
                slotIcons[i].enabled = false;
            }
        }
    }

    // Pozostawione dla kompatybilności z InventorySystem
    public void SetImmediatePosition(int activeIndex) { }
}