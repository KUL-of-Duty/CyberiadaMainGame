using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    public float speed = 5f;

    [Header("Sprint Settings")]
    public float sprintSpeed = 8f;
    private KeyCode sprintKey = KeyCode.LeftShift;


    [Header("Gravity and jump")]
    public float jumpHeight = 2f;
    public float gravityForce = 15f;
    private CharacterController characterController;
    private Vector3 velocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        // Sprint
        float currentSpeed = speed;

        if (Input.GetKey(sprintKey) && vInput > 0)
        {
            currentSpeed = sprintSpeed; // Jeśli tak, ustawiamy szybszą prędkość
        }

        // Poruszanie sie
        Vector3 move = transform.right * hInput + transform.forward * vInput;
        velocity.x = move.x * currentSpeed;
        velocity.z = move.z * currentSpeed;

        // Grawitacja i skakanie
        
        if (characterController.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -0.5f; 
            }

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravityForce);
            }
        }
        
        velocity.y += -gravityForce * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}