using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunScript : NetworkBehaviour{
    public GunObjectScript gunObjectScript;
    private bool isAttacking = false;
    private float lastAttackTime;
    private float lastReloadTime;
    private bool isReloading = false;
    PlayerAmmo ammo;
    public CinemachineCamera fpsCam;
    PlayerInput playerInput;
    InputAction shootAction;
    InputAction reloadAction;
    ParticleSystem particleSystem;
    bool isBurstActive = false;
    int burstShotsLeft;
    float nextBurstShotTime = 0f;
    
    void Awake(){
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions.FindAction("Attack");
        reloadAction = playerInput.actions.FindAction("Reload");
        particleSystem = GetComponentInChildren<ParticleSystem>();
        burstShotsLeft=gunObjectScript.burstBullets;
        lastAttackTime=Time.time;
        lastReloadTime=Time.time;
        shootAction.Enable();
        reloadAction.Enable();
    }

    public override void OnNetworkSpawn(){
        ammo = GetComponentInParent<PlayerAmmo>();
    }

    void Update(){
        if(!IsClient || !IsOwner) return;
        if (shootAction.IsPressed()){
            OnAttackPressed();
            //particleSystem.Play();
        }
        if(reloadAction.WasPressedThisFrame()){
            ReloadGun();
        }
        // --- BURST STATE MACHINE ---
        if(isBurstActive){
            if (burstShotsLeft <= 0 || ammo.Ammo.Value <= 0){
                isBurstActive = false;
            } else if (Time.time >= nextBurstShotTime){
                TryShoot();
                burstShotsLeft--;
                nextBurstShotTime = Time.time + gunObjectScript.burstInterval;
            }
        }
    }

    void Attack(){
        if(!(gunObjectScript.WeaponType==TypeOfWeapon.MELEE))
            ammo.ConsumeAmmoServerRpc(1);
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit, gunObjectScript.range)){
            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * gunObjectScript.range, Color.white,0.5f, true);
            if(hit.transform.GetComponent<Target>() == null) return;
            Target target = hit.transform.GetComponent<Target>();
            ulong id = target.NetworkObjectId;
            ReportHit(id, gunObjectScript.damage);
        }
        lastAttackTime = Time.time;
    }

    void OnAttackPressed(){ 
        if(isReloading||ammo.GetAmmo()==0) return;
        if (shootAction.WasPressedThisFrame()&&gunObjectScript.isSingleShot()){
            if(Time.time>lastAttackTime+gunObjectScript.attackInterval){
                TryShoot();
            }
        }
        else if (shootAction.WasPressedThisFrame()&&gunObjectScript.isBurstFire()){
            if (!isBurstActive && Time.time > lastAttackTime + gunObjectScript.attackInterval){
                isBurstActive = true;
                burstShotsLeft = gunObjectScript.burstBullets;
                nextBurstShotTime = Time.time+ gunObjectScript.attackInterval;
                Debug.Log(isBurstActive+" "+burstShotsLeft+" "+nextBurstShotTime);
            }
        }
        else if (shootAction.IsPressed()&&gunObjectScript.isAutoFire()){
            if (Time.time>lastAttackTime+gunObjectScript.attackInterval){
                Debug.Log("Autofire");
                TryShoot();
            }
        }
        else if (shootAction.IsPressed()&&gunObjectScript.isMelee()){
            if(Time.time>lastAttackTime+gunObjectScript.attackInterval){
                Debug.Log("Melee");
                TryShoot();
            }
        }
    }

    void TryShoot(){
        if(!IsOwner) return;
        if (gunObjectScript.WeaponType == TypeOfWeapon.MELEE){
            Attack();
        }
        else if(ammo!=null&&ammo.GetAmmo()>0){
            Attack();           
        }
    }

    [ServerRpc]
    void ReloadGunServerRpc(){
        ammo.Ammo.Value=gunObjectScript.maxAmmo;
    }

    [ServerRpc(RequireOwnership = false)]
    void ReportHitServerRpc(ulong objectId,float damage){
        if(!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject obj)) return;
        Target target = obj.GetComponent<Target>();
        if(target!=null)
            target.TakeDamage(damage);
    }

    void ReloadGun(){
        if (Time.time > gunObjectScript.reloadTime + lastReloadTime){
            lastReloadTime = Time.time;
            ReloadGunServerRpc();
        }
    }

    void ReportHit(ulong objectId,float damage){
        ReportHitServerRpc(objectId,damage);
    }
}