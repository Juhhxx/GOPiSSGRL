using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class RotateWhenHolding : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minRotation;
    [SerializeField] private float _maxRotation;
    [SerializeField] private float visualizeValue;
    private Vector3 _currentRotation;
    private void Start()
    {
        _currentRotation = transform.localRotation.eulerAngles;

        // Debug.Log($"Clamping {initRotation.z} between: {_minRotation}, {_maxRotation}");
        _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);
        // Debug.Log(initRotation.z);
        
        transform.localRotation = Quaternion.Euler(_currentRotation);

        visualizeValue = TranslateRotationIntoValue();
    }
    private float mouseMovement;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mouseMovement = Input.GetAxis("Mouse X") * _sensitivity;
            
            _currentRotation.z += mouseMovement;
            Debug.Log($"rotation z : {_currentRotation.z}");
            _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);

            transform.localRotation = Quaternion.Euler(_currentRotation);

            visualizeValue = TranslateRotationIntoValue();
        }

    }
    public float TranslateRotationIntoValue()
    {
        float rotationRatio = (_currentRotation.z - _minRotation) / (_maxRotation - _minRotation);
        return Mathf.Lerp(_minValue, _maxValue, rotationRatio);
    }
}
