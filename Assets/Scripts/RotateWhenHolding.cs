using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class RotateWhenHolding : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minRotation;
    [SerializeField] private float _maxRotation;
    private float _currentRotation;
    private void Start()
    {
        Quaternion initRotation = transform.localRotation;
        Debug.Log($"Clamping {initRotation.z} between: {_minRotation}, {_maxRotation}");
        initRotation.z = Mathf.Clamp(initRotation.z, _minRotation,_maxRotation);
        Debug.Log(initRotation.z);
        transform.localRotation = initRotation;
    }
    private void Update()
    {

    }
    private float TranslateRotationIntoValue()
    {
        float value;

        float valueDifference = _maxValue - _minValue;
        float rotationDifference = _maxRotation - _minRotation;

        value =  valueDifference * _currentRotation / rotationDifference;

        value += _minValue;

        return value;
    }
}
