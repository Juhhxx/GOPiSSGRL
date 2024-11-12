using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class UVLight : MonoBehaviour
{
    [SerializeField] private GameObject _uvSpotLightObject;
    [SerializeField] private GameObject _uvSpotLightReboundObject;
    [SerializeField] private Material _uvMaterial;
    private Light _uvLight;
    private int _lightPositionID;
    private int _spotLightDirID;
    private int _lightedID;
    private bool isOn = false;

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

        TurnOff();
    }

    // if bool isOn, which is turned on and off a the same time as necessary components
    // is true, then update the necessary variables for the uvlight to work
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            TurnOn();
        if (isOn)
        {
            Shader.SetGlobalVector(_lightPositionID, _uvLight.transform.position);
            Shader.SetGlobalVector(_spotLightDirID, -_uvLight.transform.forward);
        }
    }

    // Turns on all the necessary components to make uvlight work
    public void TurnOn()
    {
        _uvSpotLightObject.SetActive(true);
        if (_uvSpotLightReboundObject != null)
            _uvSpotLightReboundObject.SetActive(true);
        Shader.SetGlobalFloat(_lightedID, 1);
        isOn = true;
    }

    // Turns off all the necessary components to make uvlight work
    public void TurnOff()
    {
        _uvSpotLightObject.SetActive(false);
        if (_uvSpotLightReboundObject != null)
            _uvSpotLightReboundObject.SetActive(false);
        Shader.SetGlobalFloat(_lightedID, 0);
        isOn = false;
    }
}
