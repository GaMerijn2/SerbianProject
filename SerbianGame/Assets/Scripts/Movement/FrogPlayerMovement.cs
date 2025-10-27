using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrogMovement : MonoBehaviour, IHumanoidMoveable
{
    [Header("Grounding")]
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private float groundCheckOffset = 0.1f;
    [SerializeField] private float groundStickyForce = 25f;     // extra downward accel when grounded
    [SerializeField] private float groundedSmoothingTime = 0.05f; // seconds to smooth grounded state

    [Header("Hop Tuning")]
    [SerializeField] private float baseHopPower = 7f;
    [SerializeField] private float maxChargeTime = 0.6f;
    [SerializeField] private AnimationCurve chargeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float forwardPower = 6f;
    [SerializeField] private float sprintForwardMultiplier = 1.25f;
    [SerializeField] private float hopCooldown = 0.15f;

    [Header("In-Air Control")]
    [SerializeField] private float airControl = 2.5f;
    [SerializeField] private float maxAirSpeed = 6f;

    [Header("Ground Control")]
    [SerializeField] private float groundAcceleration = 25f;
    [SerializeField] private float maxGroundSpeed = 3f;
    [SerializeField] private float turnSpeed = 12f;

    [Header("Quality of Life")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;
    [SerializeField] private float dragGround = 2f;
    [SerializeField] private float dragAir = 0.1f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = false;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 desiredDirWorld;          // computed in Update, used in FixedUpdate

    private bool isCharging;
    private float chargeTimer;

    private float lastGroundedTime;
    private float lastJumpPressedTime;
    private float lastJumpReleasedTime;

    private bool isGrounded;
    private bool isGroundedSmoothed;
    private float groundedSmoothTimer;

    private bool canHop = true;
    private bool isSprinting;
    private bool isSlowWalking;
    private bool releaseBuffered;

    [SerializeField] private float walkSpeedForUI = 3f;
    public void SetMoveSpeed(float speed) { walkSpeedForUI = speed; }
    public void StopMoving() { moveInput = Vector2.zero; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (!cameraTransform && Camera.main) cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Ground check with smoothing
        Vector3 origin = transform.position + Vector3.up * groundCheckOffset;
        bool groundedNow = Physics.CheckSphere(origin, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        if (groundedNow) lastGroundedTime = Time.time;

        if (groundedNow != isGroundedSmoothed)
        {
            groundedSmoothTimer += Time.deltaTime;
            if (groundedSmoothTimer >= groundedSmoothingTime)
            {
                isGroundedSmoothed = groundedNow;
                groundedSmoothTimer = 0f;
            }
        }
        else
        {
            groundedSmoothTimer = 0f;
        }

        isGrounded = groundedNow;

        rb.linearDamping = isGroundedSmoothed ? dragGround : dragAir;

        // Charging while held
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > maxChargeTime) chargeTimer = maxChargeTime;
        }

        // Desired direction is computed in Update from latest input
        desiredDirWorld = DesiredWorldMoveDirection();

        // Buffered release resolved here if we are allowed
        bool withinCoyote = (Time.time - lastGroundedTime) <= coyoteTime;
        if (releaseBuffered && (isGroundedSmoothed || withinCoyote) && canHop)
        {
            if (debugLogs) Debug.Log("[Frog] Buffered release consumed -> hop");
            TryExecuteHop();
            releaseBuffered = false;
        }
    }

    private void FixedUpdate()
    {
        // Ground locomotion smoothing
        if (isGroundedSmoothed)
        {
            Vector3 groundVel = desiredDirWorld * maxGroundSpeed;
            Vector3 vel = rb.linearVelocity;
            Vector3 lateral = new Vector3(vel.x, 0f, vel.z);
            Vector3 delta = Vector3.ClampMagnitude(groundVel - lateral, groundAcceleration * Time.fixedDeltaTime);
            rb.AddForce(delta, ForceMode.VelocityChange);

            // keep feet planted for stickiness
            rb.AddForce(Vector3.down * groundStickyForce, ForceMode.Acceleration);
        }
        else
        {
            if (desiredDirWorld.sqrMagnitude > 0.0001f)
            {
                Vector3 vel = rb.linearVelocity;
                Vector3 lateral = Vector3.ProjectOnPlane(vel, Vector3.up);
                Vector3 wish = desiredDirWorld * maxAirSpeed;
                Vector3 add = Vector3.ClampMagnitude(wish - lateral, airControl * Time.fixedDeltaTime);
                rb.AddForce(add, ForceMode.VelocityChange);
            }
        }

        // Smooth facing, driven by physics time
        if (desiredDirWorld.sqrMagnitude > 0.0001f)
        {
            Quaternion target = Quaternion.LookRotation(desiredDirWorld, Vector3.up);
            Quaternion q = Quaternion.Slerp(rb.rotation, target, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(q);
        }
    }

    public void Move(Vector2 input)
    {
        moveInput = Vector2.ClampMagnitude(input, 1f);
    }

    public void RotateTowards(Vector2 direction)
    {
        // not used anymore; rotation handled in FixedUpdate using desiredDirWorld
    }

    public void Jump()
    {
        lastJumpPressedTime = Time.time;

        if (!isCharging)
        {
            isCharging = true;
            chargeTimer = 0f;
            if (debugLogs) Debug.Log("[Frog] Jump pressed -> start charging");
        }
    }

    public void ReleaseJump()
    {
        lastJumpReleasedTime = Time.time;

        bool canHopNow = canHop && (isGroundedSmoothed || (Time.time - lastGroundedTime) <= coyoteTime);
        if (canHopNow)
        {
            if (debugLogs) Debug.Log("[Frog] Jump released -> hop now");
            TryExecuteHop();
        }
        else
        {
            releaseBuffered = (Time.time - lastJumpPressedTime) <= jumpBufferTime;
            if (debugLogs) Debug.Log("[Frog] Jump released in air -> buffer: " + releaseBuffered);
        }
    }

    public void Crouch(bool isCrouching) { isSlowWalking = isCrouching; }
    public void Dash() { }
    public void Roll() { }
    public void Sprint(bool sprint) { isSprinting = sprint; }
    public void WalkSlow(bool isSlow) { isSlowWalking = isSlow; }

    private void TryExecuteHop()
    {
        if (!canHop) return;

        bool groundedOrCoyote = isGroundedSmoothed || (Time.time - lastGroundedTime <= coyoteTime);
        if (!groundedOrCoyote) return;

        float t = Mathf.Clamp01(chargeTimer / maxChargeTime);
        if (isSlowWalking) t *= 0.6f;
        float eval = chargeCurve.Evaluate(t);

        float vPower = baseHopPower * (0.5f + eval);
        float fPower = forwardPower * (0.5f + eval);
        if (isSprinting) fPower *= sprintForwardMultiplier;

        Vector3 dir = desiredDirWorld.sqrMagnitude > 0.0001f ? desiredDirWorld : transform.forward;

        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        Vector3 impulse = dir.normalized * fPower + Vector3.up * vPower;
        rb.AddForce(impulse, ForceMode.VelocityChange);

        if (debugLogs) Debug.Log("[Frog] HOP t=" + t.ToString("0.00") + " v=" + vPower.ToString("0.00") + " f=" + fPower.ToString("0.00"));

        canHop = false;
        Invoke(nameof(EnableHop), hopCooldown);

        isCharging = false;
        chargeTimer = 0f;
        releaseBuffered = false;
    }

    private void EnableHop() { canHop = true; }

    private Vector3 DesiredWorldMoveDirection()
    {
        if (moveInput == Vector2.zero) return Vector3.zero;
        return CameraAligned(moveInput).normalized;
    }

    private Vector3 CameraAligned(Vector2 stick)
    {
        Transform cam = cameraTransform ? cameraTransform : (Camera.main ? Camera.main.transform : transform);
        Vector3 fwd = cam.forward; fwd.y = 0f; fwd.Normalize();
        Vector3 right = cam.right; right.y = 0f; right.Normalize();
        return right * stick.x + fwd * stick.y;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGroundedSmoothed ? Color.green : Color.red;
        Vector3 origin = transform.position + Vector3.up * groundCheckOffset;
        Gizmos.DrawWireSphere(origin, groundCheckRadius);
    }
}
