using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerTimeFPSController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    private CharacterController controller;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 85f;

    [Header("Debug")]
    public bool showDebugGUI = true;
    public bool logTimeStateChanges = true;

    private float verticalVelocity;
    private float cameraPitch = 0f;

    private Vector2 moveInput;
    private bool isTryingToMove;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        ReadMovementInput();
        UpdateTimeIntent();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    void ReadMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveZ).normalized;
        isTryingToMove = moveInput.sqrMagnitude > 0.001f;
    }

    void UpdateTimeIntent()
    {
        if (TimeManager.Instance == null)
        {
            return;
        }

        if (isTryingToMove)
        {
            GameEvents.TimeStateRequested(TimeState.Normal);
        }
        else
        {
            GameEvents.TimeStateRequested(TimeState.Slow);
        }
    }

    void HandleMovement()
    {
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y);
        controller.Move(move * moveSpeed * Time.unscaledDeltaTime);

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.unscaledDeltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.unscaledDeltaTime);
    }
}
