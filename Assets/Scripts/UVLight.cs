using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class UVLight : MonoBehaviour
{
    private GameObject _spotLightObject;
    private void Start()
    {
        Light spotLight = GetComponentInChildren<Light>();
        _spotLightObject = spotLight.gameObject;
    }
    private void Use()
    {
        _spotLightObject.SetActive(true);
    }
    private void TurnOff()
    {
        _spotLightObject.SetActive(false);
    }
}
