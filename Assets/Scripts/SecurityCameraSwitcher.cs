using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject[] _securityCameras;
    [SerializeField] private bool _runningTrailer;
    private Dictionary<GameObject, Animator> _animators = new();
    private bool _running = false;
    private GameObject _currentCamera;
    private int _currentIndex = -1;

    private void Awake()
    {
        _playerCamera.SetActive(true);
    }

    private void Start()
    {
        if (!_runningTrailer) return;

        foreach (GameObject cam in _securityCameras)
        {
            _animators[cam] = cam.GetComponentInParent<Animator>();
            _animators[cam].SetFloat("MoveSpeed", 1f);
            cam.SetActive(false);
        }
    }

    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (_running)
            {
                SwitchToPlayerCamera();
            }
            else
            {
                SwitchSecurityCamera(_currentIndex >= 0 ? _currentIndex : 0);
            }
            _running = !_running;
        }

        if (!_running) return;

        if (Input.GetKeyDown(KeyCode.F9)) SwitchSecurityCamera(0);
        if (Input.GetKeyDown(KeyCode.F10)) SwitchSecurityCamera(1);
        if (Input.GetKeyDown(KeyCode.F11)) SwitchSecurityCamera(2);

        if (Input.GetKey(KeyCode.F8)) ToggleCurrentCameraMovement(true);
        else ToggleCurrentCameraMovement(false);
    }
    private float moveSpeed = 0f;
    private void ToggleCurrentCameraMovement(bool isMoving)
    {
        if (_currentCamera == null) return;

        Animator animator = _animators[_currentCamera];

        float targetMoveSpeed = isMoving ? 1f : 0f;
        moveSpeed = Mathf.Lerp(moveSpeed, targetMoveSpeed, Time.deltaTime / 1.3f);

        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    private void SwitchToPlayerCamera()
    {
        if (_currentCamera != null)
        {
            _currentCamera.SetActive(false);
            _animators[_currentCamera].SetFloat("MoveSpeed", 1f);
        }
        

        _playerCamera.SetActive(true);
        _currentCamera = null;
    }

    private void SwitchSecurityCamera(int index)
    {
        if (index < 0 || index >= _securityCameras.Length) return;

        if (_currentCamera == null)
            _playerCamera.SetActive(false);
        else
        {
            _currentCamera.SetActive(false);
            _animators[_currentCamera].SetFloat("MoveSpeed", 1f);
        }
        
        _currentCamera = _securityCameras[index];
        _currentCamera.SetActive(true);
        _currentIndex = index;
    }
    #endif
}
