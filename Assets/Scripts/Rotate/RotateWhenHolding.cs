using UnityEngine;

public class RotateWhenHolding : MonoBehaviour
{
    
    // [SerializeField] private GameObject _holdingCamera;
    private PlayerBehaviorControl _playerControl;
    [SerializeField] private float _scrollSensitivity = 45f;
    [SerializeField] private float _mouseSensitivity = 0.5f;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _minRotation;
    [SerializeField] private float _maxRotation;
    [SerializeField] private bool _wrapRotation = false;
    [SerializeField] private bool _invertRotationValues = false;
    [SerializeField] private float _currentValue;
    private Vector3 _currentRotation;
    [SerializeField] private float _visualizeCurrentRotationValue;
    private float _mouseMovement;
    private CurrentRadioRotation _currentRotationData;

    /// <summary>
    /// Gets the initial rotation, but first clamps to the min and max rotations
    /// </summary>
    private void Awake()
    {
        _playerControl = FindAnyObjectByType<PlayerBehaviorControl>();

        _currentRotationData = GetComponentInParent<CurrentRadioRotation>();
        _currentRotation = transform.localRotation.eulerAngles;
    }
    private void Start()
    {
        if (_currentRotationData != null
            && _currentRotationData.Value.HasValue)
            {
                Debug.Log("Starting");
                _currentValue = _currentRotationData.Value.Value;
            }
        else
        {
            _currentRotation.z = Mathf.Clamp(_currentRotation.z, _minRotation,_maxRotation);
            transform.localRotation = Quaternion.Euler(_currentRotation);

            float rotationRatio = (_currentRotation.z - _minRotation) / (_maxRotation - _minRotation);
            _currentValue = Mathf.Lerp(_minValue, _maxValue, rotationRatio);
        }
    }
    public void EnableRotation()
    {
        /*if (_holdingCamera != null)
            _holdingCamera.SetActive(false);*/
        
        _playerControl.EnableDisablePlayer(false);

        /*Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;*/
    }
    /// <summary>
    /// Disables player movement if the interact key was held.
    /// </summary>
    public void DisableRotation()
    {
        /*if (_holdingCamera != null)
            _holdingCamera.SetActive(true);*/
        
        _playerControl.EnableDisablePlayer(true);

        enabled = false;

        /*Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/
    }

    private void OnDisable()
    {
        if (_currentRotationData != null)
            _currentRotationData.Value = _currentValue;
    }

    private void Update()
    {
        RotateWithMouse();
        transform.localRotation = Quaternion.Euler(TranslateValueIntoRotation());
    }

    /// <summary>
    /// Update checks for mouse being pressed and then adds the mouse movement
    /// to the rotation, making sure its inside the bounds.
    /// we cannot get the transforms actual rotation and only get it in the start
    /// of the script because eular angles will not go past 0 and 360 degrees.
    ///</summary>
    private void RotateWithMouse()
    {
        if ( ! Input.GetButton("Interact")) return;

        _mouseMovement = Input.GetAxisRaw("Mouse ScrollWheel") * _scrollSensitivity * 10;

        _mouseMovement += GlobalValues.GetAxisX() * _mouseSensitivity;
        
        _currentValue += _mouseMovement;
        // Debug.Log($"rotation z : {_currentRotation.z}");

        if (_wrapRotation)
        {
            _currentValue = _minValue + (_currentValue - _minValue) % (_maxValue - _minValue);
            if (_currentValue < _minValue)
                _currentValue += _maxValue - _minValue;
        }
        _currentValue = Mathf.Clamp(_currentValue, _minValue,_maxValue);
    }

    /// <summary>
    /// This method converts the current rotation into a value between _minValue and _maxValue.
    /// It calculates the ratio of how far the current rotation is between the min and max rotations
    /// then uses Mathf.Lerp with this ratio between _minValue and _maxValue.
    /// </summary>
    /// <returns> Returns the value of same percentage as current rotation
    /// between mins and maxs.</returns>
    private Vector3 TranslateValueIntoRotation()
    {
        float valueRatio = (_currentValue - _minValue) / (_maxValue - _minValue);
        _currentRotation.z = Mathf.Lerp(_minRotation, _maxRotation, valueRatio);

        return _currentRotation;
    }

    public float GetCurrentValue()
    {
        if (_invertRotationValues)
            return _maxValue - _currentValue;
        else
            return _currentValue;
    }
}
