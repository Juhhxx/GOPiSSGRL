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

    // Start just gets the initial rotation, but first clamps to the min and max rotations
    private void Start()
    {
        _currentRotation = transform.localRotation.eulerAngles;

        // Debug.Log($"Clamping {initRotation.z} between: {_minRotation}, {_maxRotation}");
        _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);
        // Debug.Log(initRotation.z);
        
        transform.localRotation = Quaternion.Euler(_currentRotation);

        visualizeValue = TranslateRotationIntoValue();
    }

    // Update checks for mouse being pressed and then adds the mouse movement
    // to the rotation, making sure its inside the bounds.
    // we cannot get the transforms actual rotation and only get it in the start
    // of the script because eular angles will not go past 0 and 360 degrees.
    private float _mouseMovement;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _mouseMovement = Input.GetAxis("Mouse X") * _sensitivity;
            
            _currentRotation.z += _mouseMovement;
            // Debug.Log($"rotation z : {_currentRotation.z}");
            _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);

            transform.localRotation = Quaternion.Euler(_currentRotation);

            visualizeValue = TranslateRotationIntoValue();
        }

    }

    // This method converts the current rotation into a value between _minValue and _maxValue.
    // It calculates the ratio of how far the current rotation is between the min and max rotations
    // then uses Mathf.Lerp with this ratio between _minValue and _maxValue.
    public float TranslateRotationIntoValue()
    {
        float rotationRatio = (_currentRotation.z - _minRotation) / (_maxRotation - _minRotation);
        return Mathf.Lerp(_minValue, _maxValue, rotationRatio);
    }
}
