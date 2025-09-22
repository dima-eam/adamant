using UnityEngine;

public class Fadeable : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private Material[] originalMaterials;
    public Material transparentMaterial; // assign a transparent version in Inspector

    private bool isTransparent = false;

    void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalMaterials[i] = renderers[i].material; // safe copy
    }

    public void SetVisible(bool visible)
    {
        if (visible && isTransparent)
        {
            // Restore originals
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material = originalMaterials[i];

            isTransparent = false;
        }
        else if (!visible && !isTransparent)
        {
            // Apply transparent material
            foreach (var r in renderers)
                r.material = transparentMaterial;

            isTransparent = true;
        }
    }
}