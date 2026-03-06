using Unity.Netcode;
using UnityEngine;

public class ClientMovement : NetworkBehaviour
{
    [SerializeField] CharacterController _characterController;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerSoundManager _soundManager;

    private void Awake()
    {
        if (_playerMovement == null) _playerMovement = GetComponent<PlayerMovement>();
        if (_soundManager == null) _soundManager = GetComponent<PlayerSoundManager>();
        _playerMovement.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _playerMovement.enabled = true;
        }
        if (IsServer) _characterController.enabled = true;
    }

    [ServerRpc]
    public void UpdateMovementServerRPC(Vector3 move) => _characterController.Move(move);

    [ServerRpc]
    public void UpdateRotationServerRPC(Vector3 rotation) => transform.Rotate(rotation);

    [ServerRpc]
    public void UpdateGravitationServerRPC(Vector3 gravitationalPull) => _characterController.Move(gravitationalPull);

    [ServerRpc]
    public void RequestJumpSoundServerRPC() => _soundManager.PlayJumpSoundClientRPC();

    [ServerRpc]
    public void RequestStepSoundServerRPC(bool isSprinting) => _soundManager.PlayStepSoundClientRpc(isSprinting);

    [ServerRpc]
    public void TeleportPlayerServerRPC(Vector3 location)
    {
        _characterController.enabled = false;
        transform.position = location;
        _characterController.enabled = true;
    }
}