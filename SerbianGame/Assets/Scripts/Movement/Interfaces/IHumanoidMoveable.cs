using UnityEngine;

public interface IHumanoidMoveable : IMoveable
{
    void Jump();
    void Crouch(bool isCrouching);
    void Dash();
    void Roll();
    void Sprint(bool isSprinting);
    void WalkSlow(bool isSlowWalking);
    void RotateTowards(Vector2 direction);
}
