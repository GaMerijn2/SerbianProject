using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] public CinemachineCamera cam;

    private MainControls inputActions;
    private IHumanoidMoveable moveable;
    private Vector2 moveInput;

    private bool isCursorLocked = true;

    private void Awake()
    {
        moveable = GetComponent<IHumanoidMoveable>();
        inputActions = new MainControls();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Crouch.performed += ctx => moveable.Crouch(true);
        inputActions.Player.Crouch.canceled += ctx => moveable.Crouch(false);
        inputActions.Player.Sprint.performed += ctx => moveable.Sprint(true);
        inputActions.Player.Sprint.canceled += ctx => moveable.Sprint(false);
        inputActions.Player.Dash.performed += ctx => moveable.Dash();
        inputActions.Player.SlowWalk.performed += ctx => moveable.WalkSlow(true);
        inputActions.Player.SlowWalk.canceled += ctx => moveable.WalkSlow(false);
        inputActions.Player.CursorToggle.performed += ctx => OnCursorLock();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Jump.performed -= OnJump;
    }

    private void Update()
    {
        if (!isCursorLocked)
        {
            moveInput = Vector2.zero; // Prevent movement when cursor is unlocked
            moveable.Move(moveInput);

            return;
        }

        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        moveable.Move(moveInput);
        moveable.RotateTowards(moveInput);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        moveable.Jump();
    }

    private void OnCursorLock()
    {
        Debug.Log("Cursor Lock Toggled");
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cam.enabled = false;

            isCursorLocked = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cam.enabled = true;

            isCursorLocked = true;
        }
    }
}
