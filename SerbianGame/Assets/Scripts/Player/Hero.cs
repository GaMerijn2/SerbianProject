using UnityEngine;

[RequireComponent(typeof(SwordAttack))]
[RequireComponent(typeof(HarvestSystem))]
[RequireComponent(typeof(HeroEvents))]
public class Hero : MonoBehaviour
{
    public SwordAttack SwordAttack;
    public HarvestSystem HarvestSystem;
    public HeroEvents HeroEvents;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        HeroEvents.RegisterEvents();
    }

    private void OnDisable()
    {
        HeroEvents.DeRegisterEvents();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Yeetable"))
        {
            Vector3 forceDirection = Camera.main.transform.forward + Vector3.up / 2;
            other.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * 50, ForceMode.Impulse);
        }
    }
}

