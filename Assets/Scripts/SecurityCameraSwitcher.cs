using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Camera[] _securityCameras;
    [SerializeField] private bool _runningTrailer;
    private Dictionary<Camera, Animator> _animators;
    private bool _running;
    private Camera _currentCamera;
    private int _currentIndex;

    private void Awake()
    {
        _animators = new Dictionary<Camera, Animator>();
        _playerCamera.enabled = true;
        _currentCamera = _playerCamera;
        _currentIndex = 1;
    }
    private void Start()
    {
        if (!_runningTrailer) return;

        foreach (Camera cam in _securityCameras)
        {
            _animators[cam] = cam.GetComponentInParent<Animator>();
            cam.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (_running)
            {
                _playerCamera.enabled = true;
                _currentCamera = _playerCamera;
                SwitchSecurityCamera(-1);
                _running = false;
            }
            else
            {
                SwitchSecurityCamera(_currentIndex);
                _running = true;
            }
        }

        if (!_running) return;

        if (Input.GetKeyDown(KeyCode.F9))
            SwitchSecurityCamera(0);
        if (Input.GetKeyDown(KeyCode.F10))
            SwitchSecurityCamera(1);
        if (Input.GetKeyDown(KeyCode.F11))
            SwitchSecurityCamera(2);
        
        if (Input.GetKeyDown(KeyCode.F8))
            MoveCurrentCamera();
    }
    private void MoveCurrentCamera()
    {
        _animators[_currentCamera].SetBool("Move",
             !_animators[_currentCamera].GetBool("Move"));
    }

    private void SwitchSecurityCamera(int index)
    {
        if (index == -1) return;

        for (int i = 0; i < _securityCameras.Length; i++)
        {
            if (i == index)
            {
                _securityCameras[i].enabled = true;
                _currentCamera = _securityCameras[i];
            }
            else _securityCameras[i].enabled = false;
        }
    }

}
