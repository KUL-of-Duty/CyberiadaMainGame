using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Ustawienia Kamery")]
    [SerializeField] public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private Transform cameraTransform;
    
    [Header("Ustawienia Ruchu")]
    private Rigidbody rb;
    [SerializeField, Tooltip("Bazowa prędkość chodzenia.")] 
    public float WalkSpeed = 5f; 
    [SerializeField, Tooltip("Dodatkowa prędkość podczas sprintu.")] 
    public float SprintBonus = 5f; 
    [SerializeField, Tooltip("Wartość zmniejszająca prędkość podczas kucania (np. 3f).")]
    public float CrouchSpeedPenalty = 3f;
    private float currentMoveSpeed;

    private float emptyHandSpeedBonus = 2f; // Dodatkowy stały bonus do prędkości w trybie "Pusta Ręka"
    private bool isRunningEmptyHand = false; // Flaga stanu
  
    [Header("Ustawienia Kucania")]
    [SerializeField, Tooltip("Obniżenie kamery w dół o tę wartość podczas kucania.")]
    private float crouchCameraOffset = 0.5f; 
    [SerializeField, Tooltip("Szybkość, z jaką kamera płynnie zmienia wysokość.")]
    private float crouchSmoothTime = 0.1f;
    
    private bool isCrouching = false;
    private float defaultCameraY;
    private float currentCameraY;
    private float velocityY = 0.0f; 

    
    [Header("Ustawienia Skoku")]
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float fallMultiplier = 2.5f; 
    [SerializeField] public float ascendMultiplier = 2f; 
    private bool isGrounded = true;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField, Tooltip("Długość Raycasta do sprawdzania podłoża.")] 
    private float groundCheckDistance = 0.2f;

    
    private float playerHeight;
    private float raycastOriginOffset;

    [Header("Ustawienia Staminy")]
    [SerializeField]
    public float maxStamina = 100f;
    [SerializeField]
    public float staminaDrainRate = 20f;
    [SerializeField]
    public float staminaRegenRate = 10f;
    [SerializeField]
    public float regenDelay = 1.5f;

    private float currentStamina;
    private float regenTimer; 
    private bool canSprint = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            defaultCameraY = cameraTransform.localPosition.y;
            currentCameraY = defaultCameraY;
        }
        else
        {
            Debug.LogError("Zmien tag kamery na 'MainCamera'.");
        }

        if (GetComponent<Collider>() != null)
        {
            playerHeight = GetComponent<Collider>().bounds.size.y;
            raycastOriginOffset = (playerHeight / 2) - 0.05f; 
        }

        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
        HandleCrouching();
        HandleStamina();

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching) 
        {
            Jump();
        }

        CheckGrounded();
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void HandleStamina()
    {
        bool isRequestingSprint = Input.GetKey(KeyCode.LeftShift) && !isCrouching;
        bool isSprinting = isRequestingSprint && canSprint;
        
        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            regenTimer = regenDelay; 
            
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canSprint = false;
            }
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                if (regenTimer > 0)
                {
                    regenTimer -= Time.deltaTime;
                }
                else
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    currentStamina = Mathf.Min(currentStamina, maxStamina);
                }
            }

            if (currentStamina > 0f)
            {
                canSprint = true;
            }
        }
    }


   void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
        }

        float targetY = isCrouching ? defaultCameraY - crouchCameraOffset : defaultCameraY;
        currentCameraY = Mathf.SmoothDamp(currentCameraY, targetY, ref velocityY, crouchSmoothTime);
        
        if (cameraTransform != null)
        {
            Vector3 localPos = cameraTransform.localPosition;
            cameraTransform.localPosition = new Vector3(localPos.x, currentCameraY, localPos.z);
        }
    }
    // Wywoływane przez InventorySystem
    public void SetSpeedBonus(bool enableEmptyHand)
    {
        isRunningEmptyHand = enableEmptyHand;
    }

    void MovePlayer()
    {
        currentMoveSpeed = WalkSpeed;

        // 1. Sprint
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching && canSprint;
        if (isSprinting) 
        {
            currentMoveSpeed += SprintBonus;
        }

        // 2. Kucanie
        if (isCrouching)
        {
            currentMoveSpeed = Mathf.Max(0f, currentMoveSpeed - CrouchSpeedPenalty);
        }
        
        // 3. BONUS PUSTEJ RĘKI (dodatkowy bonus, gdy nie sprintuje, kuca, i ma pustą rękę)
        if (isRunningEmptyHand && !isSprinting) 
        {
            currentMoveSpeed += emptyHandSpeedBonus;
        }
        // NOTE: Jeśli chcesz, aby bonus Pustej Ręki stackował się ze sprintem,
        // usuń warunek `&& !isSprinting` w powyższym 'if'.

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveForward = Input.GetAxisRaw("Vertical");
        
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        
        Vector3 targetVelocity = new Vector3(
            movement.x * currentMoveSpeed, 
            rb.linearVelocity.y, 
            movement.z * currentMoveSpeed
        );

        rb.linearVelocity = targetVelocity;
    }

    void RotateCamera()
    {
        if (cameraTransform == null) return;

        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void CheckGrounded()
    {
        Vector3 rayOrigin = transform.position - Vector3.up * raycastOriginOffset;
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance, groundLayer);
    }

    void Jump()
    {
        isGrounded = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0) 
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        } 
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (ascendMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}