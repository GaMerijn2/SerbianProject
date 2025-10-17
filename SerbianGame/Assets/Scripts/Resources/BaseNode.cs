using UnityEngine;

public enum ResourceType
{
    None = 1,
    Stone = 2,
    GraniteStone = 3,
    CopperOre = 4,
    CoalOre = 5,
    IronOre = 6,
    GoldOre = 7,
    DiamondOre = 8,
    OakWood = 9,
    BirchWood = 10,
}

public class BaseNode : MonoBehaviour
{
    [SerializeField] private ResourceConfig resourceConfig;
    [SerializeField] private ResourceConfig specialResourceConfig;

    Resource baseResource;
    Resource specialResource;

    protected void Awake()
    {
        baseResource = resourceConfig.Generate();
        specialResource = specialResourceConfig.Generate();
    }

    public (Resource baseResource, Resource specialResource) Harvest()
    {
        return (baseResource, specialResource);
    }
}
public class Resource
{
    public ResourceType resourceType;
    public int amount;
}


