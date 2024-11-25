using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class RotateWhenHolding : MonoBehaviour
{
    
    [SerializeField] private GameObject _holdingCamera;
    private PlayerInteraction _playerInteraction;
    private PlayerMovement _playerMovement;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minRotation;
    [SerializeField] private float _maxRotation;
    private Vector3 _currentRotation;
    [SerializeField] private float _visualizeCurrentRotationValue;
    private float _mouseMovement;

    /// <summary>
    /// Gets the initial rotation, but first clamps to the min and max rotations
    /// </summary>
    private void Awake()
    {
        _playerMovement = FindFirstObjectByType<PlayerMovement>();
        _playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        
        _currentRotation = transform.localEulerAngles;

        // Debug.Log($"Clamping {initRotation.z} between: {_minRotation}, {_maxRotation}");
        _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);
        // Debug.Log(initRotation.z);
        
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }
    public void EnableRotation()
    {
        if (_holdingCamera != null)
            _holdingCamera.SetActive(false);
        
        _playerMovement.enabled = false;
        _playerInteraction.enabled = false;

        /*Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;*/
    }
    /// <summary>
    /// Disables player movement if the interact key was held.
    /// </summary>
    public void DisableRotation()
    {
        if (_holdingCamera != null)
            _holdingCamera.SetActive(true);
        
        _playerMovement.enabled = true;
        _playerInteraction.enabled = true;

        enabled = false;

        /*Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/
    }

    private void Update()
    {
        RotateWithMouse();
        _visualizeCurrentRotationValue = TranslateRotationIntoValue();
    }

    /// <summary>
    /// Update checks for mouse being pressed and then adds the mouse movement
    /// to the rotation, making sure its inside the bounds.
    /// we cannot get the transforms actual rotation and only get it in the start
    /// of the script because eular angles will not go past 0 and 360 degrees.
    ///</summary>
    private void RotateWithMouse()
    {
        _mouseMovement = Input.GetAxis("Mouse ScrollWheel") * _sensitivity;
        
        _currentRotation.z += _mouseMovement;
        // Debug.Log($"rotation z : {_currentRotation.z}");
        _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);

        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    /// <summary>
    /// This method converts the current rotation into a value between _minValue and _maxValue.
    /// It calculates the ratio of how far the current rotation is between the min and max rotations
    /// then uses Mathf.Lerp with this ratio between _minValue and _maxValue.
    /// </summary>
    /// <returns> Returns the value of same percentage as current rotation
    /// between mins and maxs.</returns>
    private float TranslateRotationIntoValue()
    {
        float rotationRatio = (_currentRotation.z - _minRotation) / (_maxRotation - _minRotation);
        return Mathf.Lerp(_minValue, _maxValue, rotationRatio);
    }

    public float GetCurrentValue()
    {
        return TranslateRotationIntoValue();
    }
}
