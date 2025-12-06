using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkObject))]
public class Mover : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 4f;             // m/s
    [SerializeField] float jumpForce = 0.005f;             // impulse
    [SerializeField] float groundCheckDistance = 1.1f; // do sprawdzania ziemi

    Rigidbody rb;
    Vector3 input = Vector3.zero;

    // Opcjonalnie throttle RPCs (prosty cooldown) -> tu na potrzeby debugowania co klatkę wysyłamy
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // rb.freezeRotation = true;
        // rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Start()
    {
        Debug.Log($"[Mover Start] NetworkObjectId={NetworkObjectId} IsOwner={IsOwner} IsServer={IsServer} IsClient={IsClient} IsLocalPlayer={IsLocalPlayer}");
    }

    void Update()
    {
        // Tylko właściciel czyta input
        if (!IsOwner) return;

        // Input w Update
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGroundedLocal())
            {
                // wysyłamy żądanie do serwera
                RequestJumpServerRpc();
                Debug.Log("[Mover] Jump button pressed -> RequestJumpServerRpc sent");
            }
            else
            {
                Debug.Log("[Mover] Jump pressed but not grounded (local check)");
            }
        }

        // Wyślij input do serwera co Update (można optymalizować)
        SendMoveInputServerRpc(input, Time.deltaTime);
    }

    // Serwer aplikuje ruch (autorytatywnie). ServerRpc wykonuje się na serwerze.
    [ServerRpc(RequireOwnership = true)]
    void SendMoveInputServerRpc(Vector3 inputFromClient, float deltaTime, ServerRpcParams rpcParams = default)
    {
        if (!IsServer) return;

        Vector3 worldDelta = (transform.right * inputFromClient.x + transform.forward * inputFromClient.z) * moveSpeed * deltaTime;
        Vector3 target = rb.position + worldDelta;

        rb.MovePosition(target);

        // do debugu na serwerze (zbyt głośne w produkcji)
        // Debug.Log($"[Server] Applied move for NetworkObjectId={NetworkObjectId}, delta={worldDelta}");
    }

    [ServerRpc(RequireOwnership = true)]
    void RequestJumpServerRpc(ServerRpcParams rpcParams = default)
    {
        if (!IsServer) return;

        if (!ServerIsGrounded())
        {
            // odrzucamy skok jeżeli serwer uważa, że obiekt nie jest na ziemi
            Debug.Log("[Server] Jump requested but server says not grounded");
            return;
        }

        rb.AddForce(Vector3.up * jumpForce);
        Debug.Log("[Server] Jump applied to rigidbody");
    }

    bool IsGroundedLocal()
    {
        // lokalny check: niedoskonały, ale daje szybką informację
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);
    }

    bool ServerIsGrounded()
    {
        // ten sam check uruchomiony po stronie serwera
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);
    }
}
