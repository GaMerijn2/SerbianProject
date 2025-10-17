using DG.Tweening;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    EventBinding<PlayerAnimationEvent> binding;

    [SerializeField] private HumanoidPlayerMovement hpm;
    [SerializeField] private HumanoidPlayerAnimations hpa;

    public Animator animator;
    public DOTweenAnimation DOTweenAnimation;

    private float smoothedSpeed = 0f;
    private float smoothVelocity = 0f;

    private float idleType = 0f;
    private float smoothedIdleType = 0f;
    private float idleTypeVelocity = 0f;

    private bool isIdle = false;
    private float idleChangeTimer = 0f;
    private float idleChangeInterval = 3f;

    private void Awake()
    {
        binding = new EventBinding<PlayerAnimationEvent>(OnPlayerAnimationEvent);
        EventBus<PlayerAnimationEvent>.Register(binding);
    }

    private void OnDestroy()
    {
        EventBus<PlayerAnimationEvent>.Deregister(binding);
    }

    private void Update()
    {
        if (animator != null)
        {
            HandleMovementBlend();
            HandleIdleVariation();
            UpdateIdleTypeSmoothing();
            ApplyAnimationParameters();
        }
    }

    private void OnPlayerAnimationEvent(PlayerAnimationEvent animEvent)
    {
        if (animator != null)
        {   
            Debug.Log("Playing Animation with Hash: " + animEvent.animationHash);
            animator.Play(animEvent.animationHash);
        }

        if (DOTweenAnimation != null)
        {
            Debug.Log("Playing DOTween Animation with ID: " + animEvent.tweenId);
            //DOTweenAnimation.DOPlayById(animEvent.tweenId);
            DOTweenAnimation.DORestartById(animEvent.tweenId);
        }
    }

    private void HandleMovementBlend()
    {
        float targetSpeed = hpm.MappedWalkSpeed;
        smoothedSpeed = Mathf.SmoothDamp(smoothedSpeed, targetSpeed, ref smoothVelocity, 0.1f);

        if (smoothedSpeed < 0.01f)
        {
            smoothedSpeed = 0f;
            isIdle = true;
        }
        else
        {
            isIdle = false;
            idleType = 0f;
            idleChangeTimer = 0f;
        }
    }

    private void HandleIdleVariation()
    {
        if (!isIdle)
            return;

        idleChangeTimer += Time.deltaTime;
        if (idleChangeTimer >= idleChangeInterval)
        {
            idleChangeTimer = 0f;
            GenerateNewIdleType();
        }
    }

    private void GenerateNewIdleType()
    {
        float random = Random.Range(0f, 100f);
        idleType = Mathf.Ceil(random);
        idleType = Mathf.Clamp(idleType, 0f, 100f);
    }

    private void UpdateIdleTypeSmoothing()
    {
        if (idleType < 1f)
            smoothedIdleType = 0f;
        else
            smoothedIdleType = Mathf.SmoothDamp(smoothedIdleType, idleType, ref idleTypeVelocity, 0.25f);
    }

    private void ApplyAnimationParameters()
    {
        hpa.SetMovementAnimationParameter("walkSpeed", smoothedSpeed);
        hpa.SetMovementAnimationParameter("idleType", smoothedIdleType);
    }
}
