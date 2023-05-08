using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TexturePass : ScriptableRenderPass
{
    
    private RenderTexture renderTexture;

    public TexturePass(RenderTexture renderTexture)
    {
        this.renderTexture = renderTexture;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // Render the renderTexture on the camera;
    }
    
     public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        cmd.GetTemporaryRT(Shader.PropertyToID("_RenderTexture"), 1920, 1080, 0, FilterMode.Bilinear);
        cmd.SetRenderTarget(renderTexture);
    }
}
