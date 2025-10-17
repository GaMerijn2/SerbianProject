using UnityEngine;

public abstract class SpellStrategy : ScriptableObject
{
    public float cooldown = 1f;
    public int spellIndex;
    public Sprite icon;
    public abstract void CastSpell(Transform origin);
}
