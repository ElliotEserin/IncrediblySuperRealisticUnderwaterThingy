using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FogEffect : MonoBehaviour
{
    public Material _mat;
    public Gradient fogColor;
    public float minHeight;
    public float maxHeight;
    public float depthStart;
    public float minLightIntensity, maxLightIntensity;
    public float depthDistance;
    public Light directionalLight;

    float modifier;
    float difference;

    private void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        difference = maxHeight - minHeight;
        modifier = 1 / difference;
    }

    void Update()
    {
        if (directionalLight == null)
            return;

        var time = (transform.position.y - minHeight) * modifier;
        directionalLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, time);
        _mat.SetColor("_FogColor", fogColor.Evaluate(0));
        _mat.SetColor("_SecondaryFogCOlor", fogColor.Evaluate(1));
        _mat.SetFloat("_DepthStart", depthStart);
        _mat.SetFloat("_DepthDistance", depthDistance);
        _mat.SetFloat("_PlayerDepth", time);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}
