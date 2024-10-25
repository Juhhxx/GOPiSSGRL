using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RevealUnderUVLight : MonoBehaviour
{
    [SerializeField] private Material _uvMaterial;
    [SerializeField] private Texture _uvTexture;
    private MeshRenderer _meshRenderer;
    private Light _uvLight;
    void Start()
    {
        UVLight uvLightObject = FindFirstObjectByType<UVLight>();
        if (uvLightObject != null)
            _uvLight = uvLightObject.GetComponentInChildren<Light>();

        if (_uvMaterial != null)
        {
            _uvMaterial.mainTexture = _uvTexture;
            _meshRenderer = GetComponent<MeshRenderer>();
            int meshMaterialCount = _meshRenderer.materials.Length;

            Material[] materials =
                new Material[meshMaterialCount +1];
            
            for (int i = 0; i < meshMaterialCount; i++)
            {
                materials[i] = _meshRenderer.materials[i];
            }
            
            materials[meshMaterialCount] = _uvMaterial;

            _meshRenderer.materials = materials;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_uvMaterial != null)
        {
            _uvMaterial.SetVector("Light Position", _uvLight.transform.position);
            _uvMaterial.SetVector("Light Direction", -_uvLight.transform.forward);
            _uvMaterial.SetFloat ("Light Angle", _uvLight.spotAngle);
        }
    }
}
