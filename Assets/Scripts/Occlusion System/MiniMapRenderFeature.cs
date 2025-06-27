using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MiniMapRenderFeature : ScriptableRendererFeature
{
    [Serializable]
    public class Settings
    {
        public string shaderPassName;
        public Material overrideMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
        public string targetName;
    }
    class MiniMapRenderPass : ScriptableRenderPass
    {
        RenderTargetIdentifier identifier;
        Settings settings;
        Material overrideMaterial;
        ShaderTagId shaderTagId;
        FilteringSettings filteringSettings;

        public MiniMapRenderPass(Settings settings)
        {
            this.settings = settings;
            overrideMaterial = settings.overrideMaterial;
            shaderTagId = new ShaderTagId(settings.shaderPassName);
            filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            this.renderPassEvent = settings.renderPassEvent;
        }

        public void SetupIdentifier(RenderTargetIdentifier identifier) => this.identifier = identifier;
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (overrideMaterial != null)
                Debug.Log($"Material not null: {overrideMaterial.name}");


            if (renderingData.cameraData.camera.name != settings.targetName)
                return;

            Debug.Log($"Camera name: {settings.targetName}");

            var drawSettings = CreateDrawingSettings(shaderTagId, ref renderingData, SortingCriteria.CommonOpaque);

           if(overrideMaterial != null)
           {
               drawSettings.overrideMaterial = overrideMaterial;
           }

           context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
     
        }

    }

    MiniMapRenderPass m_MiniMapPass;
    public Settings settings = new Settings();
    public override void Create()
    {
        m_MiniMapPass = new MiniMapRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
     
        renderer.EnqueuePass(m_MiniMapPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        m_MiniMapPass.SetupIdentifier(renderer.cameraColorTargetHandle);
    }
}


