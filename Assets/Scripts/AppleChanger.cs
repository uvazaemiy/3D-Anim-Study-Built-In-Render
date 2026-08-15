using UnityEngine;

public class AppleChanger : MonoBehaviour
{
    [SerializeField] private AppleScriptable appleConfig;

    private MeshRenderer meshRenderer;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseDown()
    {
        Material newMat = new Material(Shader.Find("Standard"));

        newMat.color = appleConfig.newColor;
        meshRenderer.material = newMat;
        
        transform.localScale = appleConfig.newScale;
    }
}
