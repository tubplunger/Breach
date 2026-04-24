using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerTimeFPSController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform firePoint;
    public GameObject projectilePrefab;

    private CharacterController controller;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 85f;

    [Header("Time Control")]
    [Range(0.01f, 1f)]
    public float movingTimeScale = 1f;

    [Range(0.01f, 1f)]
    public float idleTimeScale = 0.05f;

    public float timeChangeSpeed = 8f;

    [Header("Shooting")]
    public float fireCooldown = 0.2f;

    [Header("Debug")]
    public bool showDebugGUI = true;
    public bool logTimeStateChanges = true;

    private float verticalVelocity;
    private float cameraPitch = 0f;

    private Vector2 moveInput;
    private bool isTryingToMove;
    private float currentTargetTimeScale;
    private float fireTimer;

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

        currentTargetTimeScale = idleTimeScale;
        ApplyTimeScaleImmediate(currentTargetTimeScale);
    }

    void Update()
    {
        HandleMouseLook();
        ReadMovementInput();
        HandleMovement();
        HandleTimeControl();
        HandleShooting();
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

    void HandleTimeControl()
    {
        float previousTarget = currentTargetTimeScale;
        currentTargetTimeScale = isTryingToMove ? movingTimeScale : idleTimeScale;

        if (logTimeStateChanges && !Mathf.Approximately(previousTarget, currentTargetTimeScale))
        {
            if (isTryingToMove)
                Debug.Log("Player moving -> Time returning to normal.");
            else
                Debug.Log("Player stopped -> Time slowing down.");
        }

        float newTimeScale = Mathf.Lerp(
            Time.timeScale,
            currentTargetTimeScale,
            timeChangeSpeed * Time.unscaledDeltaTime
        );

        ApplyTimeScale(newTimeScale);
    }

    void HandleShooting()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && fireTimer >= fireCooldown)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.owner = gameObject;
        }

        Debug.Log("Player fired projectile.");
    }

    void ApplyTimeScale(float newScale)
    {
        Time.timeScale = Mathf.Clamp(newScale, 0.01f, 1f);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void ApplyTimeScaleImmediate(float newScale)
    {
        Time.timeScale = Mathf.Clamp(newScale, 0.01f, 1f);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void OnGUI()
    {
        if (!showDebugGUI) return;

        GUI.Box(new Rect(10, 10, 280, 165), "Time Debug");
        GUI.Label(new Rect(20, 40, 240, 20), "Moving Input: " + isTryingToMove);
        GUI.Label(new Rect(20, 60, 240, 20), "Target Time Scale: " + currentTargetTimeScale.ToString("F2"));
        GUI.Label(new Rect(20, 80, 240, 20), "Current Time Scale: " + Time.timeScale.ToString("F2"));
        GUI.Label(new Rect(20, 100, 240, 20), "Fixed Delta Time: " + Time.fixedDeltaTime.ToString("F4"));
        GUI.Label(new Rect(20, 120, 240, 20), "Move Input: " + moveInput);
        GUI.Label(new Rect(20, 140, 240, 20), "Fire Ready: " + (fireTimer >= fireCooldown));
    }
}
