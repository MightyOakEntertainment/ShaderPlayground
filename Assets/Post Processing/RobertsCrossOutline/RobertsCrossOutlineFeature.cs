using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RobertsCrossOutlineFeature : ScriptableRendererFeature
{
    class RobertsCrossOutlinePass : ScriptableRenderPass
    {
        private RTHandle source { get; set; }
        private RTHandle destination { get; set; }
        public Material outlineMaterial = null;
        RTHandle temporaryColorTexture;

        public void Setup(RTHandle source, RTHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public RobertsCrossOutlinePass(Material outlineMaterial)
        {
            if(outlineMaterial)
            {
                this.outlineMaterial = outlineMaterial;

            }
        }



        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {

        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("_OutlinePass");

            RenderTextureDescriptor opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDescriptor.depthBufferBits = 0;

            if (destination.nameID == k_CameraTarget.nameID)
            {

                temporaryColorTexture = RTHandles.Alloc("_temporaryColorTexture", "_temporaryColorTexture");
                cmd.GetTemporaryRT(Shader.PropertyToID(temporaryColorTexture.name), opaqueDescriptor, FilterMode.Point);
                Blit(cmd, source, temporaryColorTexture, outlineMaterial, 0);
                Blit(cmd, temporaryColorTexture, source);

            }
            else Blit(cmd, source, destination, outlineMaterial, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {

            if (destination.nameID == k_CameraTarget.nameID)
                cmd.ReleaseTemporaryRT(Shader.PropertyToID(temporaryColorTexture.name));
        }
    }

    [System.Serializable]
    public class OutlineSettings
    {
        public Material outlineMaterial = null;
    }

    public OutlineSettings settings = new OutlineSettings();
    RobertsCrossOutlinePass outlinePass;
    RTHandle outlineTexture;

    public override void Create()
    {
        outlinePass = new RobertsCrossOutlinePass(settings.outlineMaterial);
        outlinePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        outlineTexture = RTHandles.Alloc("_OutlineTexture", name: "_OutlineTexture");
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.outlineMaterial == null)
        {
            Debug.LogWarningFormat("Missing Outline Material");
            return;
        }
        renderer.EnqueuePass(outlinePass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        outlinePass.Setup(renderer.cameraColorTargetHandle, ScriptableRenderPass.k_CameraTarget);  // use of target after allocation
    }
}

