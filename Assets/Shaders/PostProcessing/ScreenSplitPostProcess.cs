using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/ScreenSplitPostProcess")]
public sealed class ScreenSplitPostProcess : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public BoolParameter enabled = new BoolParameter(false);

    [Tooltip("Second Player's render texture")]
    public RenderTextureParameter secondaryTexture = new RenderTextureParameter(null);

    [Tooltip("Axis by which to split the screen")]
    public Vector2Parameter splitAxis = new Vector2Parameter(new Vector2(0.0f, 0.0f));

    Material m_Material;

    public bool IsActive() => m_Material != null && secondaryTexture.value != null && Application.isPlaying && enabled.value;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/ScreenSplit";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume ScreenSplitPostProcess is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        // test
        m_Material.SetTexture("_SecondTex", secondaryTexture.value);
        m_Material.SetVector("_SplitAxis", splitAxis.value);

        cmd.Blit(source, destination, m_Material, 0);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
