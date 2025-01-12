using System.Collections.Generic;
using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _holdingCamera;
    [SerializeField] private SecurityCameraSwitcher _cameraSwitcher;
    [SerializeField] private PlayerBehaviorControl _playerBehaviorControl;
    [SerializeField] private CutsceneControl _cutsceneControl;
    [SerializeField] private List<Canvas> _uiToCheck;
    private List<bool> _uiSetting;
    private int _lastCameraIndex = -1;
    private void Start()
    {
        _uiSetting = new List<bool>(_uiToCheck.Count);
        for (int i = 0; i < _uiToCheck.Count; i++)
        {
            _uiSetting.Add(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pause.activeSelf && !_settings.activeSelf)
                Pause();
            else if (!_settings.activeSelf)
                Continue();
        }
    }
    public void Pause()
    {
        SaveLastSetting();

        // Time.timeScale = 0;
        _holdingCamera.SetActive(false);
        
        _playerBehaviorControl.EnableDisablePlayer(false);
        Cursor.lockState = CursorLockMode.None;

        _playerBehaviorControl.PlayPauseSpeech(true);

        _cutsceneControl.Pause(true);

        // check what was teh last camera
        _lastCameraIndex = _cameraSwitcher.IsCurrentCameraPlayer();
        SwitchToRandomCam();

        _pause.SetActive(true);
    }
    public void Continue()
    {
        SwitchToLastCam();

        _pause.SetActive(false);
        
        // Time.timeScale = 1;
        _holdingCamera.SetActive(true);
        _playerBehaviorControl.EnableDisablePlayer(true);
        Cursor.lockState = CursorLockMode.Locked;

        _playerBehaviorControl.PlayPauseSpeech(false);

        _cutsceneControl.Pause(false);

        SetLastSetting();
    }
    private void SwitchToRandomCam()
    {
        int ran = Random.Range(0, _cameraSwitcher.SecurityCameraAmount);
        _cameraSwitcher.SwitchSecurityCamera(ran);
    }
    private void SwitchToLastCam()
    {
        if (_lastCameraIndex == -1)
            _cameraSwitcher.SwitchToPlayerCamera();
        else
            _cameraSwitcher.SwitchSecurityCamera(_lastCameraIndex);
    }

    private void SaveLastSetting()
    {
        for(int i = 0; i < _uiToCheck.Count; i++)
        {
            _uiSetting[i] = _uiToCheck[i].gameObject.activeSelf;
            // Debug.Log(_uiSetting[i]);
            _uiToCheck[i].gameObject.SetActive(false);
        }
    }
    private void SetLastSetting()
    {
        for (int i = 0; i < _uiToCheck.Count; i++)
        {
            if (i >= _uiSetting.Count)
            {
                _uiSetting.Add(_uiToCheck[i].gameObject.activeSelf);
            }

            _uiToCheck[i].gameObject.SetActive(_uiSetting[i]);
        }
    }
    public void AddRemoveUIToCheck(Canvas ui, bool addOrRemove)
    {
        if (ui == null) return;

        if (addOrRemove)
            _uiToCheck.Add(ui);

        else
            _uiToCheck.Remove(ui);
    }
}
