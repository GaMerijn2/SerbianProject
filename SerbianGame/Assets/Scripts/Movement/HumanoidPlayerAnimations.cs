using UnityEngine;

public class HumanoidPlayerAnimations : MonoBehaviour
{
    [Header("Animation References")]
    [SerializeField] private Animator animator;

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void SetRunning(bool isSprinting)
    {
        animator.SetBool("isSprinting", isSprinting);
    }

    public void SetMovementAnimationParameter(string parameter, float value)
    {
        if (animator == null) 
            return;

        animator.SetFloat(parameter, value);
    }
}
