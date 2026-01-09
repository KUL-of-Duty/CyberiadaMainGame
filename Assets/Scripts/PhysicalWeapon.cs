using UnityEngine;

// Definiujemy typy slotów - widoczne w inspektorze jako lista rozwijana
public enum WeaponType { Rifle, Pistol, Melee, Utility }

public class PhysicalWeapon : MonoBehaviour
{
    [Header("Ustawienia Slotu")]
    // Wybierz typ broni z listy (Rifle=0, Pistol=1, Melee=2, Utility=3)
    public WeaponType weaponType; 
    
    [Header("UI")]
    //tu pakujemy ikonke (logiczne)
    public Sprite weaponIcon;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // Funkcja włącza/wyłącza fizykę (ważne przy podnoszeniu/wyrzucaniu)
    public void SetPhysics(bool state)
    {
        if (rb != null) rb.isKinematic = !state;
        if (col != null) col.enabled = state;
    }
}