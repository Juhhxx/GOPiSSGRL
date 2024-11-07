using UnityEngine;
using UnityEditor;

public class RevealUnderUVLight : MonoBehaviour
{
    [SerializeField] private Material _uvMaterialToInstantiate;
    private Material _uvMaterial;
    [SerializeField] private Texture _uvTexture;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        // If the uv material and uv light isnt referenced or cant be found we just disable it.
        if (_uvMaterialToInstantiate == null)
        {
            return;
        }

        _uvMaterial = new Material(_uvMaterialToInstantiate);

        // Set the (uv texture) texture that should appear on th material when
        // the uv light is shined on it
        _uvMaterial.SetTexture("_MainTex", _uvTexture);
        _meshRenderer = GetComponent<MeshRenderer>();

        // Add uv material to the mesh renderers' material array
        var materials = _meshRenderer.materials;
        System.Array.Resize(ref materials, materials.Length + 1);
        materials[^1] = _uvMaterial;
        _meshRenderer.materials = materials;
    }
}
