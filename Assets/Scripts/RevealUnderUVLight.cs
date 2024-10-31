using UnityEngine;
using UnityEditor;

public class RevealUnderUVLight : MonoBehaviour
{
    [SerializeField] private Material _uvMaterial;
    [SerializeField] private Texture _uvTexture;
    private MeshRenderer _meshRenderer;
    private Light _uvLight;

    private int _lightPositionID;
    private int _spotLightDirID;
    private int _lightedID;
    
    private Vector3 _lastLightPosition;
    private Vector3 _lastSpotLightDir;

    private bool _lightOnAndOffBuffer;

    private void Start()
    {
        UVLight uvLightObject = FindFirstObjectByType<UVLight>();

        if (uvLightObject != null)
            _uvLight = uvLightObject.GetComponentInChildren<Light>();

        // If the uv material and uv light isnt referenced or cant be found,
        // the script won't work
        // So we just disable it.
        if (_uvMaterial == null || _uvLight == null)
        {
            enabled = false;
        }

        // Set the (uv texture) texture that should appear on th material when
        // the uv light is shined on it
        _uvMaterial.SetTexture("_MainTex", _uvTexture);
        _meshRenderer = GetComponent<MeshRenderer>();

        // Add uv material to the mesh renderers' material array
        var materials = _meshRenderer.materials;
        System.Array.Resize(ref materials, materials.Length + 1);
        materials[^1] = _uvMaterial;
        _meshRenderer.materials = materials;

        // Convert the strings IDs of the Revealing Shader properties into
        // shader IDs for best performance
        _lightPositionID = Shader.PropertyToID("_SpotLightPos");
        _spotLightDirID = Shader.PropertyToID("_SpotLightDir");
        _lightedID = Shader.PropertyToID("_Lighted");

        _uvMaterial.SetFloat("_LightStrengthIntensity", _uvLight.intensity);
        _uvMaterial.SetFloat("_LightRange", _uvLight.range);
        _uvMaterial.SetFloat("_InnerSpotAngle", _uvLight.innerSpotAngle);
        _uvMaterial.SetFloat("_OuterSpotAngle", _uvLight.spotAngle);
        Debug.Log(_uvMaterial.GetFloat("_LightStrengthIntensity"));
        Debug.Log(_uvMaterial.GetFloat("_LightRange"));
        Debug.Log(_uvMaterial.GetFloat("_InnerSpotAngle"));
        Debug.Log(_uvMaterial.GetFloat("_OuterSpotAngle"));

        CacheCurrentLightValues();

        _lightOnAndOffBuffer = _uvLight.enabled;
    }

    private void Update()
    {
        // Checks if the uv light is enabled,
        // If it isn't it will return, but first it will see if the materials lighted bool is is already of,
        // if it isn't it will turn it off and check that it has changed.
        // the same logic applies to changing it to true, but it does not return this time.
        if (!_uvLight.enabled)
        {
            if (!_lightOnAndOffBuffer)
            {
                _uvMaterial.SetFloat(_lightedID, 0f);
                _lightOnAndOffBuffer = true;
            }
            return;
        }
        else
        {
            if (_lightOnAndOffBuffer)
            {
                _uvMaterial.SetFloat(_lightedID, 1f);
                _lightOnAndOffBuffer = false;
            }
        }

        // Sets properties necessary for the Revealing Shader attached to the
        // uv material, IF the values have changed
        if (HasLightChanged())
        {
            _uvMaterial.SetVector(_lightPositionID, _uvLight.transform.position);
            _uvMaterial.SetVector(_spotLightDirID, -_uvLight.transform.forward);
            CacheCurrentLightValues();
            //Selection.activeObject = _uvMaterial;
        }
        // Debug.DrawLine(_uvMaterial.GetVector(_lightPositionID), _uvMaterial.GetVector(_spotLightDirID) * _uvMaterial.GetFloat("_LightRange"), new Color(1, 0, 0, 1), Time.deltaTime);
    }

    // saves the current values of the uv light
    private void CacheCurrentLightValues()
    {
        _lastLightPosition = _uvLight.transform.position;
        _lastSpotLightDir = _uvLight.transform.forward;
    }

    // checks if the light values have changed, compared to the saved values
    private bool HasLightChanged()
    {
        return _lastLightPosition != _uvLight.transform.position ||
               _lastSpotLightDir != -_uvLight.transform.forward;
    }
}
