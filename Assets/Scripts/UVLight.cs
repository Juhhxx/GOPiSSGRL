using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class UVLight : MonoBehaviour
{
    [SerializeField] private GameObject _uvSpotLightObject;
    [SerializeField] private Material _uvMaterial;
    private Light _uvLight;
    private int _lightPositionID;
    private int _spotLightDirID;
    private int _lightedID;
    
    private Vector3 _lastLightPosition;
    private Vector3 _lastSpotLightDir;

    private bool _lightOnAndOffBuffer;

    private void Start()
    {
        // If the spotlightobject and uv light arent referenced or cant be found,
        // the script won't work
        // So we just disable it.
        if (_uvSpotLightObject == null)
        {
            enabled = false;
            return;
        }

        _uvLight = _uvSpotLightObject.GetComponent<Light>();

        if (_uvLight == null)
        {
            enabled = false;
            return;
        }

        // Convert the strings IDs of the Revealing Shader properties into
        // shader IDs for best performance
        _lightPositionID = Shader.PropertyToID("_SpotLightPos");
        _spotLightDirID = Shader.PropertyToID("_SpotLightDir");
        _lightedID = Shader.PropertyToID("_Lighted");

        Shader.SetGlobalFloat("_LightStrengthIntensity", _uvLight.intensity);
        Shader.SetGlobalFloat("_LightRange", _uvLight.range);
        Shader.SetGlobalFloat("_InnerSpotAngle", _uvLight.innerSpotAngle);
        Shader.SetGlobalFloat("_OuterSpotAngle", _uvLight.spotAngle);
        // Debug.Log(_uvMaterial.GetFloat("_LightStrengthIntensity"));
        // Debug.Log(_uvMaterial.GetFloat("_LightRange"));
        // Debug.Log(_uvMaterial.GetFloat("_InnerSpotAngle"));
        // Debug.Log(_uvMaterial.GetFloat("_OuterSpotAngle"));

        CacheCurrentLightValues();

        _lightOnAndOffBuffer = _uvLight.enabled;
    }

    private void Update()
    {
        // Checks if the uv light is enabled,
        // If it isn't it will return, but first it will see if the materials lighted bool is is already of,
        // if it isn't it will turn it off and check that it has changed.
        // the same logic applies to changing it to true, but it does not return this time.
        if (_uvLight.enabled != _lightOnAndOffBuffer)
        {
            _lightOnAndOffBuffer = _uvLight.enabled;
            Shader.SetGlobalFloat(_lightedID, _uvLight.enabled ? 1f : 0f);
        }

        // The logic bellow this one was giving issues, because shader global
        // variables apparently need to be updated every frame or the GPU will go like "nah-aw"
        // Will keep the logic for now in case i can fix it
        Shader.SetGlobalVector(_lightPositionID, _uvLight.transform.position);
        Shader.SetGlobalVector(_spotLightDirID, -_uvLight.transform.forward);

        /*// Sets properties necessary for the Revealing Shader attached to the
        // uv material, IF the values have changed
        if (HasLightChanged())
        {
            Shader.SetGlobalVector(_lightPositionID, _uvLight.transform.position);
            Shader.SetGlobalVector(_spotLightDirID, -_uvLight.transform.forward);
            CacheCurrentLightValues();
            //Selection.activeObject = _uvMaterial;
        }*/
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
    private void Use()
    {
        _uvSpotLightObject.SetActive(true);
    }
    private void TurnOff()
    {
        _uvSpotLightObject.SetActive(false);
    }
}
