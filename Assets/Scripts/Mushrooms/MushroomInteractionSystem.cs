
using UnityEngine;
using Unity.Netcode;

public class MushroomInteractionSystem : NetworkBehaviour
{
    public MushroomType mushroomType;
    public bool RandomType = false;
    [SerializeField]
    GameObject interactionbutton;
    public override void OnNetworkSpawn()
    {
        if(RandomType){
            mushroomType = (MushroomType)System.Enum.GetValues(typeof(MushroomType)).GetValue(Random.Range(0, System.Enum.GetValues(typeof(MushroomType)).Length));
        }
    }
    void Awake() 
    {
        if(!IsOwner){
            //gameObject.SetActive(false);
        }

        if(interactionbutton == null){
            Debug.LogError("Interaction button not assigned in the inspector and will be assigned automatically.");
            interactionbutton = GetComponentInChildren<BoxCollider>().gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ShowInteraction();
    }

    private void OnTriggerExit(Collider other)
    {
        HideInteraction();
    }

    private void OnTriggerStay(Collider other)
    {
        Transform target = Camera.main?.transform;
        interactionbutton.transform.LookAt(target.position);

        if (Input.GetKeyDown(KeyCode.E)){
            if(other.GetComponentInParent<NetworkObject>() == null){
                Debug.LogError("The object does not have a NetworkObject component.");
                return;
            }
            other.GetComponent<MushroomAtlas>().addToAtlas(mushroomType);
            InteractWithMushroomServerRpc(other.GetComponentInParent<NetworkObject>().OwnerClientId);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void InteractWithMushroomServerRpc(ulong playerId)
    {
        GetComponent<NetworkObject>().Despawn();
        Debug.Log($"Player {playerId} interacted with the mushroom.");
    }
    void ShowInteraction()
    {
        interactionbutton.SetActive(true);
    }

    void HideInteraction()
    {
        interactionbutton.SetActive(false);
    }
}
