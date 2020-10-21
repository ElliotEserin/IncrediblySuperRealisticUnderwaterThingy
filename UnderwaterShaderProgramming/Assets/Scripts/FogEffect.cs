﻿using System.Collections;
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
        var time = (transform.position.y - minHeight) * modifier;
        directionalLight.intensity = Mathf.Lerp(0.2f, 1f, time);
        _mat.SetColor("_FogColor", fogColor.Evaluate(time));
        _mat.SetFloat("_DepthStart", depthStart);
        _mat.SetFloat("_DepthDistance", depthDistance);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}
