using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MagicSystem/Spells/ProjectileSpellStrategy", fileName = "ProjectileSpellStrategy")]
public partial class ProjectileSpellStrategy : SpellStrategy
{
    public GameObject projectilePrefab;
    public Vector3 castOffset = new Vector3(0, 0.75f, 0);
    public float speed = 10f;
    public float duration = 10f;

    public override void CastSpell(Transform origin)
    {
        new ProjectileBuilder()
            .WithProjectilePrefab(projectilePrefab)
            .WithSpeed(speed)
            .WithDuration(duration)
            .WithOffset(castOffset)
            .Build(origin);
    }
}
