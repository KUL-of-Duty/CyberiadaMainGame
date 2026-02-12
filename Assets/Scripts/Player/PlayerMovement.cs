using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Serialize Fields")]
    [SerializeField] CinemachineCamera _playerCamera;
    [SerializeField] GameObject _groundCheck;
    [SerializeField] ClientMovement _clientMovement;

    [Header("Player movement")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _sprintSpeed = 1.8f;
    private bool _emptyHandBonus = true;
    private float _emptyHandSpeed = 1.2f;

    [Header("Player rotation")]
    [SerializeField] private float _mouseSens = 100f;
    private float _yRotation = 0f;

    [Header("Gravitation")]
    [SerializeField] private float _gravityForce = -9.8f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundRadius = 0.1f;
    private bool _isGrounded = true;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _jumpForce = 15f;
    private float _jumpVelocity = 0f;
    [SerializeField] private float _heightBounds = 21f;

    void Awake()
    {
        _clientMovement = GetComponent<ClientMovement>();
        _playerCamera = GetComponentInChildren<CinemachineCamera>();
        _playerCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner) _playerCamera.gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        _isGrounded = Physics.CheckSphere(_groundCheck.transform.position, _groundRadius, _groundLayer);

        Move();
        Rotate();
        Gravity();
        Jump();
    }

private void Move()
{
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    if ((x != 0 || z != 0) && _isGrounded)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SoundManager.PlayFootstep(SoundType.SPRINT, 0.5f); // Używamy PlayFootstep
        }
        else
        {
            SoundManager.PlayFootstep(SoundType.WALK, 0.3f);
        }
    }

    Vector3 move = transform.right * x + transform.forward * z;
    _clientMovement.UpdateMovementServerRPC(move * _speed * Time.deltaTime * Sprint());
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
        if (!_isGrounded && transform.position.y > _heightBounds)
        {
            _velocity.y += _gravityForce * Time.deltaTime;
            _clientMovement.UpdateGravitationServerRPC(_velocity * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _jumpVelocity = Mathf.Sqrt(_jumpForce * -2f * _gravityForce);
            SoundManager.PlaySound(SoundType.JUMP); // Dźwięk skoku
        }
        if(_jumpVelocity > 0)
        {
            _jumpVelocity += _gravityForce * Time.deltaTime;
            _clientMovement.UpdateMovementServerRPC(Vector3.up * _jumpVelocity * Time.deltaTime);
        }
    }

    private float Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _isGrounded)
        {
            return _sprintSpeed;
        } 
        else if (_emptyHandBonus)
        {
            return _emptyHandSpeed;
        }
        return 1.0f;
    }

    public void SetEmptyHandBonus(bool boolean) => _emptyHandBonus = boolean;
}