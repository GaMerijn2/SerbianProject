using UnityEngine;

public class BlobMovementRework : MonoBehaviour, INonHumanoidMoveable
{
    [SerializeField] private float moveSpeed = 2f;

    public void Move(Vector2 input)
    {
        Vector3 move = new Vector3(input.x, 0, input.y).normalized;
        transform.Translate(move * moveSpeed * Time.deltaTime);
    }

    public void StopMoving()
    {
        // No-op for basic AI
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void PerformSpecialMovement()
    {
        // e.g., blob jumps or slides forward
    }

    public void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
        }
    }
}
