using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GunObjectScript", menuName = "Scriptable Objects/GunObjectScript")]
public class GunObjectScript : ScriptableObject
{
    public float damage = 10f; // obrażenia
    public float range = 100f; // zasięg
    public float attackInterval= 0.34f; // czas między atakami
    public TypeOfWeapon WeaponType;
    // public bool singleShot= true; // pojedynczy strzał
    // public bool autoFire = false; // ogień automatyczny (przytrzymanie)
    // public bool melee = false; // walka wręcz
    // public bool continuousFire= false; // ogień ciągły (strumień)
    public int ammo = 30;
    public int maxAmmo = 30;
    public float reloadTime=2.35f;
    public int burstBullets=3;
    public float burstInterval = 0.125f;
}

public enum TypeOfWeapon
{
    SINGLE_SHOT, AUTO_FIRE, MELEE, BURST_FIRE
}
