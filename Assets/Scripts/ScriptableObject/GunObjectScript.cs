using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GunObjectScript", menuName = "Scriptable Objects/GunObjectScript")]
public class GunObjectScript : ScriptableObject{
    public float damage = 10f;
    public float range = 100f;
    public float attackInterval= 0.34f;
    public TypeOfWeapon weaponType;
    public int ammo = 30;
    public int maxAmmo = 30;
    public float reloadTime=2.35f;
    public int burstBullets=3;
    public float burstInterval = 0.125f;

    //07.03.2026
    //Dodaliœmy te zmienne których brakowa³o w tym SO aby dzia³a³o ze skryptami Olka [Konieczne Review z twórc¹!]
    public GameObject weaponModel;
    public int weaponIndex;
    public Sprite weaponIcon;
    //07.03.2026

    public bool isSingleShot(){
        return (weaponType==TypeOfWeapon.SINGLE_SHOT)? true:false;
    }

    public bool isAutoFire(){
        return (weaponType==TypeOfWeapon.AUTO_FIRE)? true:false;
    }

    public bool isMelee(){
        return (weaponType==TypeOfWeapon.MELEE)? true:false;
    }
    
    public bool isBurstFire(){
        return (weaponType==TypeOfWeapon.BURST_FIRE)? true:false;
    }
}