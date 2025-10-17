using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData", order = 1)]
public class AbilityData : ScriptableObject
{
    [Title("Ability Data")]
    public ActionType actionType;
    [BoxGroup("Ability Settings")]
    public float duration;
    [BoxGroup("Ability Settings")]
    public Sprite icon;
    [BoxGroup("Ability Settings")]
    public string fullName;
    [BoxGroup("Animation")]
    public AnimationClip animationClip;
    [BoxGroup("Animation")]
    public int animationHash;
    [BoxGroup("Animation")]
    public string tweenId;

    void OnValidate()
    {
        if (animationClip == null)
            return;

        animationHash = Animator.StringToHash(animationClip.name);
    }
}