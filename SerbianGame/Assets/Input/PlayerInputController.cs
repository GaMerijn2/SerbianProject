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
        inputActions.Disable();
        inputActions.Player.Enable(); // ensure just this map
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    // PlayerInputController fields
    private bool jumpHeldPrev = false;

    private void Update()
    {
        // existing cursor lock logic...
        if (!isCursorLocked)
        {
            moveInput = Vector2.zero;
            moveable.Move(moveInput);
            jumpHeldPrev = false; // reset while unlocked
            return;
        }

        // Move already handled elsewhere if you like:
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        moveable.Move(moveInput);
        moveable.RotateTowards(moveInput);

        float speed = moveInput.magnitude;

        if (inputActions.Player.Sprint.IsPressed())
            speed *= 1.25f;

        if (inputActions.Player.SlowWalk.IsPressed())
            speed *= 0.5f;

        moveable.SetMoveSpeed(speed);


        // PRESS/RELEASE EDGE DETECTION (never misses)
        bool jumpHeldNow = inputActions.Player.Jump.IsPressed();

        if (jumpHeldNow && !jumpHeldPrev)
        {
            // press edge
            moveable.Jump();
        }
        else if (!jumpHeldNow && jumpHeldPrev)
        {
            // release edge
            if (moveable is FrogMovement frog) frog.ReleaseJump();
        }

        jumpHeldPrev = jumpHeldNow;
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
