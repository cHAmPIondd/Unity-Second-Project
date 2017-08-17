using UnityEngine;
using System.Collections;

public class ScreenLine : PostEffectsBase {
    public Shader edgeSatConShader;
    private Material edgeSatConMaterial;
    public Material material
    {
        get
        {
            edgeSatConMaterial = CheckShaderAndCreateMaterial(edgeSatConShader, edgeSatConMaterial);
            return edgeSatConMaterial;
        }
    }
    [Range(0.0f, 1.0f)]
    public float edgesOnly = 0.0f;
    public Color edgeColor = Color.black;
    public Color backgroundColor = Color.white;
    public float sampleDistance = 1.0f;
    public float sensitivityDepth = 1.0f;
    public float sensitivityNormals = 1.0f;

    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
    }
    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_EdgeOnly", edgesOnly);
            material.SetColor("_EdgeColor", edgeColor);
            material.SetColor("_BackgroundColor", backgroundColor);
            material.SetFloat("_SampleDistance", sampleDistance);
            material.SetVector("_Sensitivity", new Vector4(sensitivityNormals, sensitivityDepth, 0.0f, 0.0f));
            Graphics.Blit(src, dest, material);
        }
        else
        {

            Graphics.Blit(src, dest);
        }
    }
}
