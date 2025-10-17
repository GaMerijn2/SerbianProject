using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbitalSpellStrategy", menuName = "ScriptableObjects/MagicSystem/Spells/OrbitalSpellStrategy")]
public class OrbialSpellStrategy : SpellStrategy
{
    public GameObject orbPrefab;
    public int numberOfOrbs = 3;
    public float radius = 5f;
    public float duration = 10f;
    public float rotationSpeed = 0.3f;
    public float heightOffset = 1f;

    public override void CastSpell(Transform origin)
    {
        Transform orbParent = CreateOrbParent(origin);
        RotateOrbParent(orbParent);

        for (int i = 0; i < numberOfOrbs; i++) // Fixed the loop condition from 1 < numberOfOrbs to i < numberOfOrbs
        {
            SpawnOrb(origin, orbParent, i);
        }

        Destroy(orbParent.gameObject, duration);
    }

    private void SpawnOrb(Transform origin, Transform orbParent, int i)
    {
        Instantiate(orbPrefab, CalculateSpawnPosition(origin, i), Quaternion.identity, orbParent);
    }

    private Vector3 CalculateSpawnPosition(Transform origin, int i)
    {
        float angle = i * 2f * Mathf.PI / numberOfOrbs; // Fixed angle calculation
        return origin.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }

    private void RotateOrbParent(Transform orbParent)
    {
        float rotationRate = 360f * rotationSpeed;
        DOTween.To(() => orbParent.rotation.eulerAngles, x => orbParent.rotation = Quaternion.Euler(x), new Vector3(0, rotationRate, 0), 1f)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    private Transform CreateOrbParent(Transform origin)
    {
        Transform orbParent = new GameObject("OrbParent").transform;
        orbParent.position = origin.position + new Vector3(0, heightOffset, 0);
        orbParent.rotation = origin.rotation;
        orbParent.SetParent(origin);
        return orbParent;
    }
}
