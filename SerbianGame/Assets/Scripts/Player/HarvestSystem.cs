using UnityEngine;

public class HarvestSystem : MonoBehaviour
{
    public void Harvest()
    {
        Debug.Log("Harvesting resources...");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider hitCollider in hitColliders)
        {
            BaseNode node = hitCollider.GetComponent<BaseNode>();
            if (node == null) continue;

            (Resource baseResource, Resource specialResource) allResources = node.Harvest();
            Debug.Log($"Harvested {allResources.baseResource.amount} {allResources.baseResource.resourceType} and {allResources.specialResource.amount} {allResources.specialResource.resourceType}");
        }
    }
}

