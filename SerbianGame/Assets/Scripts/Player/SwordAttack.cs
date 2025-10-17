using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private SphereCollider swordCollider;

    [Button(ButtonSizes.Medium), GUIColor(0.8f, 0.3f, 0.3f)]
    public void EnableSwordCollider()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        swordCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        swordCollider.gameObject.SetActive(false);
    }
}

