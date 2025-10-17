using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float duration = 10f;
    void Start()
    {
        Destroy(gameObject, duration);
    }
}
