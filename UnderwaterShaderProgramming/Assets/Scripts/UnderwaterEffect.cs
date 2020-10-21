using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class UnderwaterEffect : MonoBehaviour
{
    public Material _mat;
    [Range(0.001f, 0.1f)]
    public float _pixelOffset;
    [Range(0.1f, 20f)]
    public float _noiseScale;
    [Range(0.1f, 20f)]
    public float _noiseFrequency;
    [Range(0.1f, 30f)]
    public float _noiseSpeed;

    public float depthStart;
    public float depthDistance;

    void Update()
    {
        _mat.SetFloat("_NoiseFrequency", _noiseFrequency);
        _mat.SetFloat("_NoiseSpeed", _noiseSpeed);
        _mat.SetFloat("_NoiseScale", _noiseScale);
        _mat.SetFloat("_PixelOffset", _pixelOffset);

        _mat.SetFloat("_DepthStart", depthStart);
        _mat.SetFloat("_DepthDistance", depthDistance);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}
