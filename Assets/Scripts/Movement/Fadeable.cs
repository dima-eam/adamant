using UnityEngine;
/// <summary>
///     Manages any environment object which is "fadeable". That is, if such an object is between player and camera, its material changes to a
///     transparent one.
/// </summary>
public class Fadeable : MonoBehaviour
{
	// assign a transparent version in Inspector
	public Material transparentMaterial;

	private bool isTransparent;
	private Material[] originalMaterials;
	private MeshRenderer[] renderers;

	private void Awake()
	{
		renderers = GetComponentsInChildren<MeshRenderer>();
		originalMaterials = new Material[renderers.Length];
		for (var i = 0; i < renderers.Length; i++)
			originalMaterials[i] = renderers[i].material;
	}

	internal void SetVisible(bool visible)
	{
		if (visible && isTransparent)
		{
			// Restore originals
			for (var i = 0; i < renderers.Length; i++)
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
