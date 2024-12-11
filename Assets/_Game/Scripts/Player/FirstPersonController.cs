using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float sprintMulti = 2;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float gravityMulti = 1;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;


    //private variables
    private Vector3 currentMovement;
    private float verticalRotation;
    private float currentSpeed => walkSpeed * (playerInputHandler.sprintTriggered ? sprintMulti : 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Ensure Camera and character controller are valid
        CheckReferencesAreValid();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// Calculate normalised worled direction
    /// </summary>
    /// <returns>current facing world direction normalised</returns>
    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.movementInput.x, 0f, playerInputHandler.movementInput.y);
        Vector3 worldDirecion = transform.TransformDirection(inputDirection);
        return worldDirecion.normalized;
    }

    /// <summary>
    /// Handle Jumping
    /// </summary>
    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (playerInputHandler.jumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMulti * Time.deltaTime;
        }
    }

    /// <summary>
    /// Handle Movement
    /// </summary>
    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * currentSpeed;
        currentMovement.z = worldDirection.z * currentSpeed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    /// <summary>
    /// Handle rotation
    /// </summary>
    /// <param name="rotationAmount"></param>
    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }


    /// <summary>
    /// Apply vertical rotation
    /// </summary>
    /// <param name="rotationAmount">clamped vertical rotation</param>
    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    /// <summary>
    /// handle rotation
    /// </summary>
    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.rotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.rotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }

    /// <summary>
    /// Check references are valid
    /// </summary>
    private void CheckReferencesAreValid()
    {
        //check to make sure camera is properly referenced
        if (mainCamera == null)
        {
            mainCamera = GetComponentInChildren<Camera>();
        }
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }
}
