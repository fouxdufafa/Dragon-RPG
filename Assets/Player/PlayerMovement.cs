using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputMode inputMode = InputMode.Mouse;

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    [SerializeField] const float walkableStoppingDistance = 0.5f;
    [SerializeField] const float enemyStoppingDistance = 1.5f;

    ThirdPersonCharacter character;
    NavMeshAgent navMeshAgent;
    AICharacterControl aiCharacterControl;
    CameraRaycaster cameraRaycaster;
    GameObject walkTarget;
    
    // TODO: Implement jump
    // TODO: Fix slowness issues with gamepad movement
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        character = GetComponent<ThirdPersonCharacter>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiCharacterControl = GetComponent<AICharacterControl>();
        walkTarget = new GameObject("WalkTarget");

        cameraRaycaster.notifyMouseClickObservers += OnClickPriorityLayer;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        HandleInputModeToggle();

        switch (inputMode)
        {
            case InputMode.Mouse:
                //ProcessMouseMovement();
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
        if (inputMode != InputMode.Mouse)
        {
            return;
        }

        switch (layerHit)
        {
            case walkableLayerNumber:
                navMeshAgent.stoppingDistance = walkableStoppingDistance;
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;

            case enemyLayerNumber:
                navMeshAgent.stoppingDistance = enemyStoppingDistance;
                aiCharacterControl.SetTarget(raycastHit.transform);
                break;
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

    private void StopMovement()
    {
        aiCharacterControl.SetTarget(gameObject.transform);
    }
}

