using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityCameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera[] _playerCameras;
    [SerializeField] private AudioListener[] _playerListeners;
    [SerializeField] private GameObject[] _securityCameras;
    public int SecurityCameraAmount => _securityCameras.Length; 
    [SerializeField] private bool _runningTrailer;
    private Dictionary<GameObject, Animator> _animators = new();
    private bool _running = false;
    private GameObject _currentCamera;
    private int _currentIndex = -1;

    [SerializeField] private Image _static;

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
                SwitchSecurityCamera(_currentIndex >= 0 ? _currentIndex : 0, false);
            }
            _running = !_running;
        }

        if (!_running) return;

        if (Input.GetKeyDown(KeyCode.F9)) SwitchSecurityCamera(0, false);
        if (Input.GetKeyDown(KeyCode.F10)) SwitchSecurityCamera(1, false);
        if (Input.GetKeyDown(KeyCode.F11)) SwitchSecurityCamera(2, false);

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

    public void SwitchToPlayerCamera()
    {
        if (_currentCamera != null)
        {
            _currentCamera.SetActive(false);
            _animators[_currentCamera].SetFloat("MoveSpeed", 1f);
        }
        

        TurnPlayer(true);
        _currentCamera = null;

        _static.enabled = false;
    }

    public void SwitchSecurityCamera(int index, bool keepStatic = true)
    {
        if (index < 0 || index >= _securityCameras.Length) return;

        if (_currentCamera == null)
        {
            TurnPlayer(false);
        }
        else
        {
            _currentCamera.SetActive(false);
            _animators[_currentCamera].SetFloat("MoveSpeed", 1f);
        }
        
        _currentCamera = _securityCameras[index];
        _currentCamera.SetActive(true);
        _currentIndex = index;

        if (keepStatic)
            _static.enabled = true;
        else
            StartCoroutine(StartStatic());
    }

    private IEnumerator StartStatic()
    {
        Debug.Log("Started temporary glitch. ");

        float passedTime = 0f;

        while (passedTime < 0.18f)
        {
            _static.enabled = !_static.enabled;

            passedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.22f - passedTime);
        }

        _static.enabled = false;
    }

    /// <summary>
    /// Use this to see if in pause menu you need to switch the camera back or not.
    /// </summary>
    /// <returns> Returns -1 if the last camera is player or the index of the last security camera. </returns>
    public int IsCurrentCameraPlayer()
    {
        return _currentCamera == null ? -1 : _currentIndex;
    }

    public Camera GetCurrenIndexCam()
    {
        return _securityCameras[_currentIndex].GetComponentInChildren<Camera>();
    }
}
