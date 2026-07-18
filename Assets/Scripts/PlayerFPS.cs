using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerFPS : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform cameraPlace;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upperLookLimit = -80f;
    [SerializeField] private float lowerLookLimit = 80f;

    public Transform CameraPlace => cameraPlace;

    
    private CharacterController _characterController;
    private Vector3 _velocity;
    private float _verticalRotation = 0f;
    private bool _isGrounded;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null && mainCam.transform.IsChildOf(transform))
            {
                playerCamera = mainCam.transform;
            }
            else
            {
                playerCamera = GetComponentInChildren<Camera>()?.transform;
            }
        };
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleMovement()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; 
        }

        float moveX = 0f;
        float moveZ = 0f;

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveZ += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveZ -= 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveX -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveX += 1f;
        }

        Vector3 inputDir = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 move = transform.right * inputDir.x + transform.forward * inputDir.z;

        _characterController.Move(move * moveSpeed * Time.deltaTime);

        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        if (playerCamera == null) 
            return;

        var mouse = Mouse.current;
        if (mouse == null) 
            return;

        Vector2 mouseDelta = mouse.delta.ReadValue();
        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, upperLookLimit, lowerLookLimit);

        playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }
}
