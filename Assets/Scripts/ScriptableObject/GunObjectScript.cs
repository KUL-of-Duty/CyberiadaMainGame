using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GunObjectScript", menuName = "Scriptable Objects/GunObjectScript")]
public class GunObjectScript : ScriptableObject{
    public float damage = 10f;
    public float range = 100f;
    public float attackInterval= 0.34f;
    public TypeOfWeapon WeaponType;
    public int ammo = 30;
    public int maxAmmo = 30;
    public float reloadTime=2.35f;
    public int burstBullets=3;
    public float burstInterval = 0.125f;

    public bool isSingleShot(){
        return (WeaponType==TypeOfWeapon.SINGLE_SHOT)? true:false;
    }

    public bool isAutoFire(){
        return (WeaponType==TypeOfWeapon.AUTO_FIRE)? true:false;
    }

    public bool isMelee(){
        return (WeaponType==TypeOfWeapon.MELEE)? true:false;
    }
    
    public bool isBurstFire(){
        return (WeaponType==TypeOfWeapon.BURST_FIRE)? true:false;
    }
}