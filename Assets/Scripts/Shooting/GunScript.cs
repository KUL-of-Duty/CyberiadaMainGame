using Unity.Cinemachine;
// using Unity.Entities.UniversalDelegates;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class GunScript : NetworkBehaviour
{
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
    int burstShotsLeft = 0;
    float nextBurstShotTime = 0f;

    bool _fallingEnabled = true;

    /*void Start()
    {
        Debug.Log($"[GunScript Start] NetworkObjectId={NetworkObjectId} IsOwner={IsOwner} IsServer={IsServer} IsClient={IsClient} IsLocalPlayer={IsLocalPlayer}");
    }
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions.FindAction("Attack");
        reloadAction = playerInput.actions.FindAction("Reload");
        particleSystem = GetComponent<ParticleSystem>();
        lastAttackTime=Time.time;
        lastReloadTime=Time.time;
        shootAction.Enable();
        reloadAction.Enable();
    }

    public override void OnNetworkSpawn()
    {
        ammo = GetComponentInParent<PlayerAmmo>();
    }
    */
    void Update()
    {



        /*
            if(!IsClient || !IsOwner) return;
                // --- START ATTACK ---
                if (shootAction.IsPressed()){
                OnAttackPressed(); // ustawia isBurstActive i burstShotsLeft jeśli BURST
                Debug.Log("Attack");
                particleSystem.Play();
            }

            // --- RELOAD ---
            if(reloadAction.WasPressedThisFrame()){
                ReloadGun();
                Debug.Log("Reload");
            }

            // --- BURST STATE MACHINE ---
            if(isBurstActive){
                if (burstShotsLeft <= 0 || gunObjectScript.ammo <= 0){
                    isBurstActive = false;
                } else if (Time.time >= nextBurstShotTime){
                    TryShoot();
                    burstShotsLeft--;
                    nextBurstShotTime = Time.time + gunObjectScript.burstInterval;
                }
            }
        }


            void Attack()
            {
                //Debug.Log($"Atak na {NetworkObjectId}");
                RaycastHit hit;
                if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit, gunObjectScript.range))
                {
                    Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * gunObjectScript.range, Color.white,0.5f, true);
                    // Debug.Log(hit.transform.name);
                    // Debug.Log(gunObjectScript.ammo);
                    Target target = hit.transform.GetComponent<Target>();
                    Debug.Log(target);
                    if(target==null) return;
                    ulong id = target.NetworkObjectId;

                    ReportHit(id, gunObjectScript.damage);
                }
                lastAttackTime = Time.time;

            }
            void OnAttackPressed(){

                    if (shootAction.WasPressedThisFrame()&&gunObjectScript.WeaponType== FireMode.SINGLE_SHOT){
                        if(Time.time>lastAttackTime+gunObjectScript.attackInterval)
                        {
                            TryShoot();
                        }
                    }
                    else if (shootAction.WasPressedThisFrame()&&gunObjectScript.WeaponType == FireMode.BURST_FIRE)
                    {
                        if (!isBurstActive && Time.time > lastAttackTime + gunObjectScript.attackInterval)
                        {
                            isBurstActive = true;
                            burstShotsLeft = gunObjectScript.burstBullets;
                            nextBurstShotTime = Time.time;
                        }
                    }
                    else if (gunObjectScript.WeaponType== FireMode.AUTO_FIRE){
                        if (Time.time>lastAttackTime+gunObjectScript.attackInterval)
                        {
                            TryShoot();
                        }
                    }
                    else if (gunObjectScript.WeaponType== FireMode.MELEE){
                        if(Time.time>lastAttackTime+gunObjectScript.attackInterval){
                            TryShoot();
                            }
                    }

            }
            void TryShoot(){
                if(!IsOwner) return;
                Debug.Log("Player: "+NetworkObjectId+" Ammo: "+ammo.Ammo.Value);
                if (gunObjectScript.WeaponType != FireMode.MELEE)
                {
                    Attack();
                }
                else if(ammo!=null&&ammo.Ammo.Value>0){
                    ammo.ConsumeAmmoServerRpc(1);
                    Attack();
                }
            }
            [ServerRpc]
            void ReloadGunServerRpc(){
                //if (Time.time > gunObjectScript.reloadTime + lastReloadTime){
                    Debug.Log($"Reload na {NetworkObjectId}");
                    ammo.Ammo.Value=gunObjectScript.maxAmmo;
                //}
            }
            [ServerRpc]
            void ShootServerRpc(ServerRpcParams rpc = default){
                Debug.Log("ShootServerrpc");
                Attack();
            }
            [ServerRpc(RequireOwnership = false)]
            void ReportHitServerRpc(ulong objectId,float damage){
                Debug.Log("ReportHitServerRpc");
                Debug.Log(!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId,out var o));
                if(!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject obj)) return;
                Target target = obj.GetComponent<Target>();
                 Debug.Log(target);
                if(target!=null)
                    target.TakeDamage(damage);
            }

            void ReloadGun(){
                //if (Time.time > gunObjectScript.reloadTime + lastReloadTime){
                    Debug.Log($"Reload na {NetworkObjectId}");
                    ammo.Ammo.Value=gunObjectScript.maxAmmo;
                    ReloadGunServerRpc();
                //}
            }

            void ReportHit(ulong objectId,float damage){
                Debug.Log("ReportHit");
                Debug.Log(!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId,out var o));
                ReportHitServerRpc(objectId,damage);
            } */
    }
}

