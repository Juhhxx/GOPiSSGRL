using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class UVLight : MonoBehaviour
{
    [SerializeField] private GameObject _uvSpotLightObject;
    [field:SerializeField] public Light UVSpotLight { get; private set; }
    private void Start()
    {
        if (_uvSpotLightObject == null)
        {
            Light spotLight = GetComponentInChildren<Light>();
            _uvSpotLightObject = spotLight.gameObject;
        }
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
