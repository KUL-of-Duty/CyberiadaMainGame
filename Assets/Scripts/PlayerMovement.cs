using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencje")]
    public Transform cameraTarget; 

    [Header("Ustawienia Ruchu")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 8f;
    public float emptyHandBonus = 2f;

    [Header("Kamera")]
    public float sensitivity = 2f;
    private float verticalRotation = 0f;

    [Header("Fizyka i Podłoże")]
    public LayerMask groundLayer;
    public Transform groundCheck;     // Obiekt na dole stóp
    public float groundDistance = 0.3f; // Promień sfery sprawdzającej podłoże
    public float jumpCooldown = 0.15f;  // Czas blokady sprawdzania ziemi po skoku
    
    private Rigidbody rb;
    private bool isGrounded;
    private bool hasEmptyHandBonus;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Blokujemy rotację żeby postać się nie przewracała
        rb.freezeRotation = true;
        
        // Interpolacja => płynny ruch kamery i postaci
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleRotation();
        
        // Skok tylko gdy jesteśmy na ziemi
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Obrót lewo-prawo (postać)
        transform.Rotate(Vector3.up * mouseX);

        // Obrót góra-dół (kamera)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cameraTarget.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Wybór prędkości
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        
        // Dodanie bonusu za puste ręce
        if (hasEmptyHandBonus) speed += emptyHandBonus;

        // Obliczanie kierunku w zależności do obrotu postaci
        Vector3 moveDir = (transform.forward * v + transform.right * h).normalized;
        
        // Nadawanie prędkości przy zachowaniu grawitacji
        rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
    }

    void Jump()
    {
        //Resetujemy prędkość pionową, żeby skok nie kumulował pędu
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        // Nadajemy siłę impulsu w górę
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        // Zapisujemy czas skoku i natychmiast wyłączamy isGrounded
        lastJumpTime = Time.time;
        isGrounded = false;
    }

    void CheckGround()
    {
        //po prostu zrobienie punktu odpowiedzialnego za "stopy"
        if (Time.time > lastJumpTime + jumpCooldown)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        }
    }
    
    // Metoda wywoływana z InventorySystem przy zmianie broni na dłoń i odwrotnie
    public void SetEmptyHandBonus(bool active)
    {
        hasEmptyHandBonus = active;
    }

}