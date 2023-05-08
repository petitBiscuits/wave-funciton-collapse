using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TextureFeature : ScriptableRendererFeature
{
    private TexturePass customPass;
    

    public override void Create()
    {
        customPass = new TexturePass(RenderTexture.GetTemporary(1920, 1080));
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(customPass);
    }

}
