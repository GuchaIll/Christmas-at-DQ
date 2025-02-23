using UnityEngine;

public class PostProcessingRenderer : MonoBehaviour {
    
    [SerializeField]
    private Material postprocessMaterial;

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth | DepthTextureMode.DepthNormals;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postprocessMaterial);
    }
}