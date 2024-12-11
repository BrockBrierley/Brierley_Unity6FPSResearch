using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //IA to read from
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;
    
    //IA reference string for player
    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    //IA reference name Strings
    [Header("Action Map Name Reference")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";

    //IA to read player input from
    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    //Value read from IA
    public Vector2 movementInput { get; private set; }
    public Vector2 rotationInput { get; private set; }
    public bool jumpTriggered { get; private set; }
    public bool sprintTriggered { get; private set; }


    private void Awake()
    {
        //action map to read input from
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        //set up input read from IA
        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);

        //Subsribe to IA events
        SubscirbeActionValuesToInputEvents();
    }

    /// <summary>
    /// Subscribe to events in input action for the player
    /// </summary>
    private void SubscirbeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => movementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => movementInput = Vector2.zero;

        rotationAction.performed += inputInfo => rotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => rotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => jumpTriggered = true;
        jumpAction.canceled += inputInfo => jumpTriggered = false;

        sprintAction.performed += inputInfo => sprintTriggered = true;
        sprintAction.canceled += inputInfo => sprintTriggered = false;
    }



    //Set enable and disable to ensure IA arent running and calling actions when not properly set up.
    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}
