using Unity.Cinemachine;
// using Unity.Entities.UniversalDelegates;
using Unity.Netcode;
using UnityEngine;

public class gunScript : NetworkBehaviour
{
    public GunObjectScript gunObjectScript;
    private bool isAttacking = false;
    private float lastAttackTime;
    private float lastReloadTime;
    private bool isReloading = false;
    PlayerAmmo ammo;
    public CinemachineCamera fpsCam;

    void Start()
    {
        Debug.Log($"[GunScript Start] NetworkObjectId={NetworkObjectId} IsOwner={IsOwner} IsServer={IsServer} IsClient={IsClient} IsLocalPlayer={IsLocalPlayer}");
    }
    void Awake()
    {
        //gunObjectScript = GetComponent<GunObjectScript>();
        lastAttackTime=Time.time;
        lastReloadTime=Time.time;
        //gunObjectScript.range=gunObjectScript.WeaponType==TypeOfWeapon.BURST_FIRE?2f:gunObjectScript.range;
    }

    public override void OnNetworkSpawn()
    {
        ammo = GetComponentInParent<PlayerAmmo>();
    }
    void Update(){
        if(IsClient && IsOwner){
            if (Input.GetMouseButtonDown(0))
                OnAttackPressed();
            if(Input.GetKeyDown("r"))
                ReloadGunServerRpc();
        }
        // else if(IsServer&&IsOwner){
        //     OnAttackPressed();
        //     ReloadGunServerRpc();
        // }
        
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
            
            ReportHitServerRpc(id, gunObjectScript.damage);
        }
        lastAttackTime = Time.time;
        
    }
    void OnAttackPressed(){
            if (gunObjectScript.WeaponType==TypeOfWeapon.SINGLE_SHOT){
                if (Time.time>lastAttackTime+gunObjectScript.attackInterval)
                {
                    TryShoot();
                }
            }
            else if (gunObjectScript.WeaponType==TypeOfWeapon.BURST_FIRE){
                int firedBullets = 0;
                if(Time.time>lastAttackTime+gunObjectScript.attackInterval)
                    isAttacking = true;
                while (isAttacking)
                {
                    if(gunObjectScript.ammo==0||firedBullets==gunObjectScript.burstBullets) isAttacking=false;
                    if (firedBullets < gunObjectScript.burstBullets && Time.time > lastAttackTime + gunObjectScript.burstInterval)
                    {
                        TryShoot();
                        firedBullets++;
                    }
                }
            }
            else if (gunObjectScript.WeaponType==TypeOfWeapon.AUTO_FIRE){
                if(Time.time>lastAttackTime+gunObjectScript.attackInterval)
                {
                    TryShoot();
                }
            }
            else if (gunObjectScript.WeaponType==TypeOfWeapon.MELEE){
                if(Time.time>lastAttackTime+gunObjectScript.attackInterval)
                    TryShoot();
            }
    }
    void TryShoot(){
        if(!IsOwner) return;
        //Debug.Log("Player: "+NetworkObjectId+" Ammo: "+ammo.Ammo.Value);
        if(ammo!=null&&ammo.Ammo.Value>0){
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
}
