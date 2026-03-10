using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Serialize Fields")]
    [SerializeField] CinemachineCamera _playerCamera;
    [SerializeField] GameObject _groundCheck;
    [SerializeField] ClientMovement _clientMovement;
    [SerializeField] PlayerSoundManager _soundManager; // reference
    [SerializeField] GameObject playerRIG;
    [SerializeField] GameObject playerHUD;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _sprintSpeed = 1.8f;
    [SerializeField] private float _emptyHandSpeed = 1.2f;
    private bool _emptyHandBonus = true;

    [Header("Rotation & Physics")]
    [SerializeField] private float _mouseSens = 100f;
    [SerializeField] private float _gravityForce = -9.8f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundRadius = 0.1f;
    [SerializeField] private float _jumpForce = 15f;

    [Header("Footstep Settings")]
    [SerializeField] private float _walkStepInterval = 0.5f;
    [SerializeField] private float _sprintStepInterval = 0.3f;
    
    private float _yRotation = 0f;
    private bool _isGrounded = true;
    private Vector3 _velocity = Vector3.zero;
    private float _jumpVelocity = 0f;
    private bool _jumpRequested = false;
    private float _stepTimer = 0f;
    private bool _wasGrounded = true;

    void Awake()
    {
        _clientMovement = GetComponent<ClientMovement>();
        _playerCamera = GetComponentInChildren<CinemachineCamera>();
        _playerCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _playerCamera.gameObject.SetActive(true);
            playerHUD.gameObject.SetActive(true);
            playerRIG.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) _jumpRequested = true;

        Rotate();
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        _wasGrounded = _isGrounded;
        _isGrounded = Physics.CheckSphere(_groundCheck.transform.position, _groundRadius, _groundLayer);

        Move();
        Gravity();
        Jump();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * x + transform.forward * z;

        float multiplier = Sprint();
        _clientMovement.UpdateMovementServerRPC(moveDirection * _speed * Time.fixedDeltaTime * multiplier);

        bool isMoving = new Vector2(x, z).sqrMagnitude >= 0.01f;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && _isGrounded;
        HandleFootsteps(isMoving, isSprinting);
    }

    private void Rotate()
    {
        float x = Input.GetAxis("Mouse X") * _mouseSens * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * _mouseSens * Time.deltaTime;

        _yRotation -= y;
        _yRotation = Mathf.Clamp(_yRotation, -60, 50);

        _playerCamera.transform.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        _clientMovement.UpdateRotationServerRPC(Vector3.up * x);
    }

    private void Gravity()
    {
        if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;
        if (!_isGrounded)
        {
            _velocity.y += _gravityForce * Time.fixedDeltaTime;
            _clientMovement.UpdateGravitationServerRPC(_velocity * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        if (_jumpRequested)
        {
            _jumpVelocity = Mathf.Sqrt(_jumpForce * -2f * _gravityForce);
            _jumpRequested = false;
            _clientMovement.RequestJumpSoundServerRPC();
        }

        if (_jumpVelocity > 0)
        {
            _jumpVelocity += _gravityForce * Time.fixedDeltaTime;
            _clientMovement.UpdateMovementServerRPC(Vector3.up * _jumpVelocity * Time.fixedDeltaTime);
        }
    }

    private void HandleFootsteps(bool isMoving, bool isSprinting)
    {
        if (!_isGrounded)
        {
            _stepTimer = 0f;
            return;
        }

        // Just landed: wait one interval before first step to avoid stacked sounds
        if (_isGrounded && !_wasGrounded)
        {
            _stepTimer = isSprinting ? _sprintStepInterval : _walkStepInterval;
            return;
        }

        if (!isMoving)
        {
            _stepTimer = 0f;
            return;
        }

        _stepTimer -= Time.fixedDeltaTime;
        if (_stepTimer > 0f) return;

        _clientMovement.RequestStepSoundServerRPC(isSprinting);
        _stepTimer = isSprinting ? _sprintStepInterval : _walkStepInterval;
    }

    private float Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _isGrounded) return _sprintSpeed;
        return _emptyHandBonus ? _emptyHandSpeed : 1.0f;
    }
    
    public void SetEmptyHandBonus(bool status)
    {
        _emptyHandBonus = status;
    }
}