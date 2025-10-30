using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Custom/Ben Day Bloom")]
[SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
public class BenDayBloomEffectComponent : VolumeComponent, IPostProcessComponent
{
    [Header("Bloom Settings")]
    public FloatParameter threshold = new FloatParameter(0.9f, true);
    public FloatParameter intensity = new FloatParameter(1f, true);
    public ClampedFloatParameter scatter = new ClampedFloatParameter(0.7f, 0, 1, true);
    public IntParameter clamp = new IntParameter(65472, true);
    public ClampedIntParameter maxIteration = new ClampedIntParameter(6, 0, 10);
    public NoInterpColorParameter tint = new NoInterpColorParameter(Color.white);

    [Header("Ben-Day Settings")]
    public IntParameter dotsDensity = new IntParameter(10, true);
    public ClampedFloatParameter dotsCutoff = new ClampedFloatParameter(0.4f, 0, 1, true);
    public Vector2Parameter scrollDirection = new Vector2Parameter(Vector2.zero);

    // IMPORTANT: return true when the effect should run
    public bool IsActive() => (intensity.value > 0f) || (dotsDensity.value > 0);
    public bool IsTileCompatible() => false;
}
