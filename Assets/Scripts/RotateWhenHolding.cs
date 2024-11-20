using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class RotateWhenHolding : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minRotation;
    [SerializeField] private float _maxRotation;
    private PlayerMovement _player;
    private Vector3 _currentRotation;
    private float _mouseMovement;

    /// <summary>
    /// Start just gets the initial rotation, but first clamps to the min and max rotations
    /// </summary>
    private void Start()
    {
        _player = FindFirstObjectByType<PlayerMovement>();
        
        _currentRotation = transform.localRotation.eulerAngles;

        // Debug.Log($"Clamping {initRotation.z} between: {_minRotation}, {_maxRotation}");
        _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);
        // Debug.Log(initRotation.z);
        
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    /// <summary>
    /// Make sure PlayerMovement is enabled even if, for example, the player
    /// changes inventory item, and this script is disabled.
    /// </summary>
    private void OnDisable()
    {
        EnableMovement();
    }

    private void Update()
    {
        DisableMovement();
        RotateWithMouse();
        EnableMovement();
    }
    /// <summary>
    /// Disables player movement if the interact key was held.
    /// </summary>
    private void EnableMovement()
    {
        if (!Input.GetButtonUp("Use")) return;
        _player.enabled = true;
        /*Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/
    }
    /// <summary>
    /// Disables player movement if the interact key was released.
    /// </summary>
    private void DisableMovement()
    {
        if (!Input.GetButtonDown("Use")) return;
        _player.enabled = false;
        /*Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;*/
    }

    /// <summary>
    /// Update checks for mouse being pressed and then adds the mouse movement
    /// to the rotation, making sure its inside the bounds.
    /// we cannot get the transforms actual rotation and only get it in the start
    /// of the script because eular angles will not go past 0 and 360 degrees.
    ///</summary>
    private void RotateWithMouse()
    {
        if (Input.GetButton("Use"))
        {
            _mouseMovement = Input.GetAxis("Mouse X") * _sensitivity;
            
            _currentRotation.z += _mouseMovement;
            // Debug.Log($"rotation z : {_currentRotation.z}");
            _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);

            transform.localRotation = Quaternion.Euler(_currentRotation);
        }
    }

    /// <summary>
    /// This method converts the current rotation into a value between _minValue and _maxValue.
    /// It calculates the ratio of how far the current rotation is between the min and max rotations
    /// then uses Mathf.Lerp with this ratio between _minValue and _maxValue.
    /// </summary>
    /// <returns> Returns the value of same percentage as current rotation
    /// between mins and maxs.</returns>
    public float TranslateRotationIntoValue()
    {
        float rotationRatio = (_currentRotation.z - _minRotation) / (_maxRotation - _minRotation);
        return Mathf.Lerp(_minValue, _maxValue, rotationRatio);
    }
}
