using UnityEngine;
using Unity.Netcode;

public class HeliDropdown : NetworkBehaviour
{
    [SerializeField] GameObject dropLocation;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.K) && other.GetComponent<NetworkObject>().IsOwner) 
        {
            other.GetComponent<ClientMovement>().TeleportPlayerServerRPC(dropLocation.transform.position);
        }
    }
}
