using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencje")]
    // Przeciągnij tutaj Transform kamery lub jej rodzica
    public Transform cameraTarget; 

    [Header("Ustawienia Ruchu")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2f;
    public float jumpForce = 8f;
    public float emptyHandBonus = 2f; // Dodatkowa prędkość, gdy nic nie trzymasz

    [Header("Kamera")]
    public float sensitivity = 2f;
    private float verticalRotation = 0f;

    [Header("Fizyka")]
    public LayerMask groundLayer; // Ustaw warstwę "Ground" dla podłoża
    private Rigidbody rb;
    private bool isGrounded;
    private bool isCrouching;
    private bool hasEmptyHandBonus;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Zapobiega przewracaniu się gracza
        Cursor.lockState = CursorLockMode.Locked; // Blokuje kursor na środku
    }

    void Update()
    {
        HandleRotation(); // Obsługa myszki
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching) Jump();
        HandleCrouch(); // Obsługa kucania
    }

    void FixedUpdate()
    {
        Move(); // Fizyczny ruch postaci
        CheckGround(); // Sprawdzanie czy stoisz na ziemi
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX); // Obrót lewo-prawo
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f); // Blokada patrzenia pionowo
        cameraTarget.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float speed = walkSpeed;
        if (isCrouching) speed = crouchSpeed;
        else if (Input.GetKey(KeyCode.LeftShift)) speed = sprintSpeed;

        // Dodaje bonus prędkości jeśli flaga hasEmptyHandBonus jest aktywna
        if (hasEmptyHandBonus) speed += emptyHandBonus;

        Vector3 moveDir = (transform.forward * v + transform.right * h).normalized;
        rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
    }

    void HandleCrouch()
    {
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        float targetY = isCrouching ? 0.8f : 1.6f; // Zmiana wysokości kamery
        cameraTarget.localPosition = new Vector3(0, Mathf.Lerp(cameraTarget.localPosition.y, targetY, Time.deltaTime * 10f), 0);
    }

    void Jump() => rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    
    // Raycast w dół sprawdza, czy dotykasz warstwy groundLayer
    void CheckGround() => isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f, groundLayer);
    
    // Metoda wywoływana przez InventorySystem
    public void SetEmptyHandBonus(bool active) => hasEmptyHandBonus = active;
}