using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class UVLight : MonoBehaviour
{
    [SerializeField] private GameObject _uvSpotLightObject;
    [SerializeField] private GameObject _uvSpotLightReboundObject;
    [SerializeField] private Material _uvMaterial;
    [SerializeField] private Sounds _sounds;
    [SerializeField] private AudioSource _audioSource;
    private Light _uvLight;
    private int _lightPositionID;
    private int _spotLightDirID;
    private int _lightedID;
    private bool isOn;
    private AudioClip _click;
    private WaitWhile _waitUntilFinishPlaying;
    private bool _isDisabling;

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

        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        _waitUntilFinishPlaying = new WaitWhile(() => _audioSource.isPlaying);

        // Convert the strings IDs of the Revealing Shader properties into
        // shader IDs for best performance
        _lightPositionID = Shader.PropertyToID("_SpotLightPos");
        _spotLightDirID = Shader.PropertyToID("_SpotLightDir");
        _lightedID = Shader.PropertyToID("_Lighted");

        Shader.SetGlobalFloat("_LightStrengthIntensity", _uvLight.intensity);
        Shader.SetGlobalFloat("_LightRange", _uvLight.range);
        Shader.SetGlobalFloat("_InnerSpotAngle", _uvLight.innerSpotAngle);
        Shader.SetGlobalFloat("_OuterSpotAngle", _uvLight.spotAngle);

        _click = _sounds.GetSound(SoundID.FlashLightClick);

        _uvSpotLightObject.SetActive(false);
        if (_uvSpotLightReboundObject != null)
            _uvSpotLightReboundObject.SetActive(false);
        Shader.SetGlobalFloat(_lightedID, 0);
        isOn = false;
    }
    private void OnEnable()
    {
        if (isOn)
        {
            _uvSpotLightObject.SetActive(false);
            if (_uvSpotLightReboundObject != null)
                _uvSpotLightReboundObject.SetActive(false);
            Shader.SetGlobalFloat(_lightedID, 0);
            isOn = false;
        }
        _isDisabling = false;
    }

    /// <summary>
    /// If not already disabling, Start disabling coroutine.
    /// </summary>
    private void OnDisable()
    {
        if (!_isDisabling)
        {
            if (!gameObject.activeSelf) return;
            _isDisabling = true;
            StartCoroutine(DisableWithDelay());
        }
    }

    /// <summary>
    /// Disable the object only after the click sound stops playing.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableWithDelay()
    {
        TurnOff();

        yield return _waitUntilFinishPlaying;

        gameObject.SetActive(false);
    }

    /// <summary>
    /// if bool isOn, which is turned on and off a the same time as necessary components
    /// is true, then update the necessary variables for the uvlight to work.
    /// </summary>
    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetButtonDown("Interact"))
            ToggleUVLight();
        #endif

        if (isOn)
        {
            Shader.SetGlobalVector(_lightPositionID, _uvLight.transform.position);
            Shader.SetGlobalVector(_spotLightDirID, -_uvLight.transform.forward);
        }
    }

    /// <summary>
    /// Turns the Light on if its off and off if its on
    /// </summary>
    public void ToggleUVLight()
    {
        isOn = isOn ? TurnOff() : TurnOn();
    }

    /// <summary>
    /// Turns on all the necessary components to make uvlight work
    /// </summary>
    /// <returns> Returns that the light functions were turned on. </returns>
    private bool TurnOn()
    {
        _audioSource.PlayOneShot(_click);

        _uvSpotLightObject.SetActive(true);
        if (_uvSpotLightReboundObject != null)
            _uvSpotLightReboundObject.SetActive(true);
        Shader.SetGlobalFloat(_lightedID, 1);

        return true;
    }

    /// <summary>
    /// Turns off all the necessary components to make uvlight work
    /// </summary>
    /// <returns> Returns that the light functions were turned off. </returns>
    private bool TurnOff()
    {
        _audioSource.PlayOneShot(_click);

        _uvSpotLightObject.SetActive(false);
        if (_uvSpotLightReboundObject != null)
            _uvSpotLightReboundObject.SetActive(false);
        Shader.SetGlobalFloat(_lightedID, 0);

        return false;
    }
}
