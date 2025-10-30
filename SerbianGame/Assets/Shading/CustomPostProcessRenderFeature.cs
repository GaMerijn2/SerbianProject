using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader m_bloomShader;
    [SerializeField] private Shader m_compositeShader;

    private Material m_bloomMaterial;
    private Material m_compositeMaterial;

    private CustomPostProcessPass m_customPass;

    public override void Create()
    {
        if (m_bloomShader == null || m_compositeShader == null)
        {
            Debug.LogWarning("[CustomPostProcess] Shaders not assigned.");
            return;
        }

        m_bloomMaterial = CoreUtils.CreateEngineMaterial(m_bloomShader);
        m_compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);

        if (m_bloomMaterial == null || m_compositeMaterial == null)
        {
            Debug.LogWarning("[CustomPostProcess] Materials could not be created.");
            return;
        }

        m_customPass = new CustomPostProcessPass(m_bloomMaterial, m_compositeMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (m_customPass == null) return;
        if (m_bloomMaterial == null || m_compositeMaterial == null) return;

        // Only run for the Game camera
        if (renderingData.cameraData.cameraType != CameraType.Game) return;

        renderer.EnqueuePass(m_customPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (m_customPass == null) return;
        if (renderingData.cameraData.cameraType != CameraType.Game) return;

        m_customPass.ConfigureInput(ScriptableRenderPassInput.Color);
        m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth);

        // Valid on the classic (non-RenderGraph) path
        m_customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_bloomMaterial);
        CoreUtils.Destroy(m_compositeMaterial);
    }
}
