using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera[] _playerCameras;
    [SerializeField] private AudioListener[] _playerListeners;
    [SerializeField] private GameObject[] _securityCameras;
    [SerializeField] private GameObject[] _uis;
    [SerializeField] private bool _runningTrailer;
    private Dictionary<GameObject, Animator> _animators = new();
    private bool _running = false;
    private GameObject _currentCamera;
    private int _currentIndex = -1;

    private void Start()
    {
        if (!_runningTrailer)
        {
            gameObject.SetActive(false);
            return;
        }
        
        foreach (GameObject cam in _securityCameras)
        {
            _animators[cam] = cam.GetComponentInParent<Animator>();
            _animators[cam].SetFloat("MoveSpeed", 1f);
            cam.SetActive(false);
        }
    }

    #if UNITY_EDITOR
    private void Awake()
    {
        TurnPlayer(true);
    }
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

        if (Input.GetKey(KeyCode.LeftArrow)) ToggleCurrentCameraMovement(2f);
        else if (Input.GetKey(KeyCode.RightArrow)) ToggleCurrentCameraMovement(-2f);
        else ToggleCurrentCameraMovement(0f);
    }
    private float moveSpeed = 0f;
    private void ToggleCurrentCameraMovement(float dir)
    {
        if (_currentCamera == null) return;

        Animator animator = _animators[_currentCamera];

        moveSpeed = Mathf.Lerp(moveSpeed, dir, Time.deltaTime / 1.3f);

        animator.SetFloat("MoveSpeed", moveSpeed);
    }
    private void TurnUIs(bool on = true)
    {
        foreach (GameObject ui in _uis)
        {
            ui.SetActive(on);
        }
    }
    private void TurnPlayer(bool on)
    {
        foreach (Camera component in _playerCameras)
        {
            component.enabled = on;
        }
        foreach (AudioListener component in _playerListeners)
        {
            component.enabled = on;
        }
    }
    #endif

    private void SwitchToPlayerCamera()
    {
        if (_currentCamera != null)
        {
            _currentCamera.SetActive(false);
            _animators[_currentCamera].SetFloat("MoveSpeed", 1f);
        }
        

        TurnPlayer(true);
        _currentCamera = null;
        TurnUIs(true);
    }

    private void SwitchSecurityCamera(int index)
    {
        if (index < 0 || index >= _securityCameras.Length) return;

        if (_currentCamera == null)
        {
            TurnPlayer(false);
            TurnUIs(false);
        }
        else
        {
            _currentCamera.SetActive(false);
            _animators[_currentCamera].SetFloat("MoveSpeed", 1f);
        }
        
        _currentCamera = _securityCameras[index];
        _currentCamera.SetActive(true);
        _currentIndex = index;
    }
}
