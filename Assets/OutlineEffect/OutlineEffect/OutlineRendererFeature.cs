using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace cakeslice
{
    // Uses the existing Outline components and camera settings in URP's Render Graph.
    public sealed class OutlineRendererFeature : ScriptableRendererFeature
    {
        private OutlinePass pass;

        public override void Create()
        {
            pass = new OutlinePass
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing,
                requiresIntermediateTexture = true
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var camera = renderingData.cameraData.camera;
            if (camera.cameraType == CameraType.Game &&
                camera.TryGetComponent<OutlineEffect>(out var effect) &&
                effect.isActiveAndEnabled && effect.HasOutlines)
            {
                renderer.EnqueuePass(pass);
            }
        }

        private sealed class OutlinePass : ScriptableRenderPass
        {
            private sealed class PassData
            {
                public TextureHandle source, mask, maskDepth, filteredMask, destination;
                public OutlineEffect effect;
                public Camera camera;
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                var resources = frameData.Get<UniversalResourceData>();
                var camera = frameData.Get<UniversalCameraData>().camera;
                var effect = camera.GetComponent<OutlineEffect>();
                if (resources.isActiveTargetBackBuffer || effect == null || !effect.isActiveAndEnabled)
                    return;

                var colorDesc = renderGraph.GetTextureDesc(resources.activeColorTexture);
                colorDesc.name = "Outlined Camera Color";
                colorDesc.msaaSamples = MSAASamples.None;
                colorDesc.depthBufferBits = DepthBits.None;
                var destination = renderGraph.CreateTexture(colorDesc);

                var maskDesc = colorDesc;
                maskDesc.name = "Outline Mask";
                maskDesc.colorFormat = GraphicsFormat.R8G8B8A8_UNorm;
                maskDesc.filterMode = FilterMode.Point;
                var mask = renderGraph.CreateTexture(maskDesc);
                var depthDesc = maskDesc;
                depthDesc.name = "Outline Mask Depth";
                depthDesc.depthBufferBits = DepthBits.Depth16;
                var maskDepth = renderGraph.CreateTexture(depthDesc);
                maskDesc.name = "Filtered Outline Mask";
                var filteredMask = effect.addLinesBetweenColors
                    ? renderGraph.CreateTexture(maskDesc) : TextureHandle.nullHandle;

                using (var builder = renderGraph.AddUnsafePass<PassData>("Tomato Outline", out var data))
                {
                    data.source = resources.activeColorTexture;
                    data.mask = mask;
                    data.maskDepth = maskDepth;
                    data.filteredMask = filteredMask;
                    data.destination = destination;
                    data.effect = effect;
                    data.camera = camera;
                    builder.UseTexture(data.source, AccessFlags.Read);
                    builder.UseTexture(mask, AccessFlags.ReadWrite);
                    builder.UseTexture(maskDepth, AccessFlags.WriteAll);
                    builder.UseTexture(destination, AccessFlags.WriteAll);
                    if (filteredMask.IsValid())
                        builder.UseTexture(filteredMask, AccessFlags.ReadWrite);
                    builder.AllowGlobalStateModification(true);
                    builder.SetRenderFunc((PassData d, UnsafeGraphContext context) =>
                    {
                        var cmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);
                        RTHandle source = d.source;
                        RTHandle outlineMask = d.mask;
                        RTHandle outlineDepth = d.maskDepth;
                        RTHandle output = d.destination;
                        CoreUtils.SetRenderTarget(cmd, outlineMask, outlineDepth, ClearFlag.All, Color.clear);
                        d.effect.RecordOutlines(cmd, d.camera);
                        cmd.SetGlobalTexture("_OutlineSource", outlineMask.nameID);
                        if (d.filteredMask.IsValid())
                        {
                            RTHandle filtered = d.filteredMask;
                            cmd.Blit(source.nameID, filtered.nameID, d.effect.outlineShaderMaterial, 0);
                            cmd.SetGlobalTexture("_OutlineSource", filtered.nameID);
                        }
                        cmd.Blit(source.nameID, output.nameID, d.effect.outlineShaderMaterial, 1);
                    });
                }
                resources.cameraColor = destination;
            }
        }
    }
}
