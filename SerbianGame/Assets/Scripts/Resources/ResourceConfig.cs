using UnityEngine;


[CreateAssetMenu(fileName = "NewResourceConfig", menuName = "Resources/ResourceConfig")]
public class ResourceConfig : ScriptableObject
{
    public ResourceType resourceType;
    public int maxAmount;

    public Resource Generate()
    {
        return new Resource
        {
            resourceType = resourceType,
            amount = maxAmount
        };
    }
}


