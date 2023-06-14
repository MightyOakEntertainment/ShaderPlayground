using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Pen&Ink/Ben Day Bloom", typeof(UniversalRenderPipeline))]
public class BenDayBloomEffectComponent : VolumeComponent, IPostProcessComponent
{
    [Header("Bloom Settings")]
    public FloatParameter threshold = new FloatParameter(0.9f, true);
    public FloatParameter intensity = new FloatParameter(1f, true);
    public ClampedFloatParameter scatter = new ClampedFloatParameter(0.7f, 0f, 1f, true);

    public IntParameter clamp = new IntParameter(65472, true);
    public ClampedIntParameter maxIterations = new ClampedIntParameter(6, 0, 10);

    public NoInterpColorParameter tint = new NoInterpColorParameter(Color.white);

    [Header("Benday")]
    public IntParameter dotDensity = new IntParameter(10, true);

    public ClampedFloatParameter dotCutoff = new ClampedFloatParameter(0.4f, 0f, 1f, true);

    public Vector2Parameter scrollDirection = new Vector2Parameter(new Vector2());

    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }

}
