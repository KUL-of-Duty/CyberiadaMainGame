using Unity.Netcode;
using UnityEngine;

public class ClientMovement : NetworkBehaviour
{
    [Header("Player Controllers")]
    [SerializeField]
    CharacterController _characterController;
    [SerializeField]
    PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement.enabled = false;
        _characterController.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            _playerMovement.enabled = true;
            _characterController.enabled = true;
        }

        if (IsServer)
        {
            _characterController.enabled = true;
        }

    }

    [ServerRpc]
    public void UpdateMovementServerRPC(Vector3 move)
    {
        _characterController.Move(move);    
    }

    [ServerRpc]
    public void UpdateRotationServerRPC(Vector3 rotation)
    {
        transform.Rotate(rotation);
    }

    [ServerRpc]
    public void UpdateGravitationServerRPC(Vector3 gravitationalPull)
    {
        _characterController.Move(gravitationalPull);
    }
}
