using System.Collections;
using UnityEngine;

public class MeltIce : MonoBehaviour
{
    [SerializeField] private RotateBridge _thermostat;
    [SerializeField] private float _minTemp;
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_animator == null) Debug.Log("Animator for Ice not found. ");
    }
    public void BeginCheckToMelt()
    {
        StartCoroutine(CheckTemperature());
    }

    private IEnumerator CheckTemperature()
    {
        while (true)
        {
            Debug.Log("thermo " + _thermostat.GetCurrentValue());
            if (_thermostat.GetCurrentValue() > _minTemp)
            {
                _animator.SetTrigger("Melt");
                break;
            }
            yield return null;
        }
    }
}
