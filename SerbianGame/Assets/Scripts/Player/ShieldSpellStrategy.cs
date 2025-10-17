using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MagicSystem/Spells/ShieldSpellStrategy", fileName = "ShieldSpellStrategy")]
public class ShieldSpellStrategy : SpellStrategy
{
    public GameObject shieldPrefab;
    public float duration = 10f;

    public override void CastSpell(Transform origin)
    {
        GameObject obj = Instantiate(shieldPrefab, origin.position + new Vector3(0, 0.75f, 0), Quaternion.identity, origin);
        obj.AddComponent<DestroyObject>().duration = duration;
    }
}
