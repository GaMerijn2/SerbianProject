using UnityEngine;

public interface IMoveable
{
    void Move(Vector2 input);
    void StopMoving();
    void SetMoveSpeed(float speed);
}
