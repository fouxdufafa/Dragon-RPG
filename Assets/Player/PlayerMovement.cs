using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputMode inputMode = InputMode.Mouse;
    [SerializeField] static float Epsilon = 0.1f;
    [SerializeField] float MoveRadiusThreshold = 0f;
    [SerializeField] float AttackRadiusThreshold = 5f;

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
    
    // TODO: Implement jump
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnClickPriorityLayer;
        character = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        HandleInputModeToggle();

        switch (inputMode)
        {
            case InputMode.Mouse:
                ProcessMouseMovement();
                break;

            case InputMode.Gamepad:
                ProcessGamepadMovement();
                break;

            default:
                // Undefined movement mode
                break;
        }
        
    }

    void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit)
    {
        clickPoint = raycastHit.point;
        print("Cursor raycast hit " + layerHit);
        switch (layerHit)
        {
            case walkableLayerNumber:
                currentDestination = ShortDestination(clickPoint, MoveRadiusThreshold);
                break;

            case enemyLayerNumber:
                currentDestination = ShortDestination(clickPoint, AttackRadiusThreshold);
                break;
        }
    }

    private void ProcessMouseMovement()
    {
        Vector3 moveDirection = currentDestination - transform.position;
        if (moveDirection.magnitude > Epsilon)
        {
            character.Move(moveDirection, false, false);
        }
        else
        {
            // Avoid infinite circling by ignoring small movements
            StopMovement();
        }
    }

    private void ProcessGamepadMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = v * cameraForward + h * Camera.main.transform.right;

        character.Move(moveDirection, false, false);
    }

    private void HandleInputModeToggle()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StopMovement();

            // TODO: Cycle through a queue to future-proof against more input modes
            if (inputMode == InputMode.Gamepad)
            {
                print("Switching to mouse");
                inputMode = InputMode.Mouse;
            }
            else if (inputMode == InputMode.Mouse)
            {
                print("Switching to gamepad");
                inputMode = InputMode.Gamepad;
            }
        }
    }

    private Vector3 ShortDestination(Vector3 destination, float scaleFactor)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * scaleFactor;
        return destination - reductionVector;
    }

    private void StopMovement()
    {
        character.Move(Vector3.zero, false, false);
        currentDestination = transform.position;
    }

    private void OnDrawGizmos()
    {
        if (inputMode == InputMode.Gamepad)
        {
            return;
        }

        // Draw movement gizmos
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, clickPoint);
        Gizmos.DrawSphere(currentDestination, 0.15f);
        Gizmos.DrawSphere(clickPoint, 0.1f);

        // Draw attack sphere
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, AttackRadiusThreshold);
    }
}

