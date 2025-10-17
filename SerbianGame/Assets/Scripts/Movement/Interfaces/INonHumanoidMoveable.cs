using UnityEngine;

public interface INonHumanoidMoveable : IMoveable
{
    void PerformSpecialMovement(); // e.g., slither, fly, crawl
    void RotateTowards(Vector3 direction);
}
