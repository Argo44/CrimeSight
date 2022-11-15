using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// This script was written using this tutorial:
// https://www.youtube.com/watch?v=ehyMwVnnnTg
// This class is meant to combine the outline effect with the main camera

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.AfterStack, "Outline Composite")]
public sealed class PostProcessComposite : PostProcessEffectSettings
{
    public ColorParameter color = new ColorParameter { value = Color.white };
}

public class PostProcessOutlineComposite : PostProcessEffectRenderer<PostProcessComposite>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/OutlineComposite"));
        sheet.properties.SetColor("_Color", settings.color);

        if (PostProcessOutlineRenderer.outlineRendererTexture != null)
            sheet.properties.SetTexture("_OutlineTex", PostProcessOutlineRenderer.outlineRendererTexture);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
