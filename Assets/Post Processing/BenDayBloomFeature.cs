using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BenDayBloomFeature : ScriptableRendererFeature
{
    class BenDayBloomPass : ScriptableRenderPass
    {
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    [SerializeField]
    private Shader m_bloomShader;
    [SerializeField]
    private Shader m_compositeShader;
    
    BenDayBloomPass m_benDayBloomPass;

    public override void Create()
    {
        m_benDayBloomPass = new BenDayBloomPass();

        // Configures where the render pass should be injected.
        m_benDayBloomPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_benDayBloomPass);
    }
}


