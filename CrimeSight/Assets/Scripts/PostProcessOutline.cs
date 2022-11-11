using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// This script was written using this tutorial:
// https://www.youtube.com/watch?v=ehyMwVnnnTg

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.AfterStack, "Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings
{
    // Fields
    public FloatParameter thickness = new FloatParameter { value = 1f };
    public FloatParameter depthMin = new FloatParameter { value = 0f };
    public FloatParameter depthMax = new FloatParameter { value = 1f };
}
