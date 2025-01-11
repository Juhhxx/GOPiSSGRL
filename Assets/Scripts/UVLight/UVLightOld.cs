using UnityEngine;
using UnityEngine.UI;

public class UVLightOld : MonoBehaviour
{
    [SerializeField] private GameObject _uvSpotLightObject;
    [SerializeField] private Light _uvLight;
    [SerializeField] private Sounds _sounds;
    [SerializeField] private AudioSource _audioSource;
    private int _lightPositionID;
    private int _spotLightDirID;
    private int _lightedID;
    private bool isOn;
    private AudioClip _click;
    private Collider _necroCollider;
    private PlayerBehaviorControl _playerControl;

    private void Start()
    {
        _playerControl = FindAnyObjectByType<PlayerBehaviorControl>();

        // Convert the strings IDs of the Revealing Shader properties into
        // shader IDs for best performance
        _lightPositionID = Shader.PropertyToID("_SpotLightPos");
        _spotLightDirID = Shader.PropertyToID("_SpotLightDir");
        _lightedID = Shader.PropertyToID("_Lighted");

        Shader.SetGlobalFloat("_LightStrengthIntensity", _uvLight.intensity);
        Shader.SetGlobalFloat("_LightRange", _uvLight.range);
        Shader.SetGlobalFloat("_InnerSpotAngle", Mathf.Cos(0.5f * Mathf.Deg2Rad * _uvLight.innerSpotAngle));
        Shader.SetGlobalFloat("_OuterSpotAngle", Mathf.Cos(0.5f * Mathf.Deg2Rad * _uvLight.spotAngle));
        Shader.SetGlobalColor("_SpotLightColor", _uvLight.color);

        // Here to test the old shader graph
        Shader.SetGlobalFloat("_InnerSpotAngleOld", _uvLight.innerSpotAngle);
        Shader.SetGlobalFloat("_OuterSpotAngleOld", _uvLight.spotAngle);

        _click = _sounds.GetSound(SoundID.FlashLightClick);

        _uvSpotLightObject.SetActive(true);
        Shader.SetGlobalFloat(_lightedID, 1);
        isOn = true;


        TagNecro necro = FindAnyObjectByType<TagNecro>();
        if (necro != null)
            _necroCollider = necro.GetComponent<Collider>();
        if (_necroCollider != null)
            _necroCollider.enabled = false;
    }

    private void OnEnable()
    {
        if (isOn)
        {
            Shader.SetGlobalFloat(_lightedID, 1);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && _playerControl.CanInteract())
            ToggleUVLight();

        if (isOn)
        {
            Shader.SetGlobalVector(_lightPositionID, _uvLight.transform.position);
            Shader.SetGlobalVector(_spotLightDirID, -_uvLight.transform.forward);
        }
    }

    public void ToggleUVLight()
    {
        isOn = isOn ? TurnOff() : TurnOn();
    }

    private bool TurnOn()
    {
        _audioSource.PlayOneShot(_click);

        _uvSpotLightObject.SetActive(true);
        Shader.SetGlobalFloat(_lightedID, 1);

        if (_necroCollider != null)
            _necroCollider.enabled = true;
        
        return true;
    }

    private bool TurnOff()
    {
        _audioSource.PlayOneShot(_click);

        _uvSpotLightObject.SetActive(false);
        Shader.SetGlobalFloat(_lightedID, 0);

        if (_necroCollider != null)
            _necroCollider.enabled = false;

        return false;
    }

    private void OnDisable()
    {
        Shader.SetGlobalFloat(_lightedID, 0);
    }
}
