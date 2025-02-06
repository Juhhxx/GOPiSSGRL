using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _holdingCamera;
    [SerializeField] private SecurityCameraSwitcher _cameraSwitcher;
    [SerializeField] private PlayerBehaviorControl _playerBehaviorControl;
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private List<GameObject> _uiToCheck;

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
        if (_holdingCamera != null)
            _holdingCamera.SetActive(false);

        if (_playerBehaviorControl != null)
        {
            _playerBehaviorControl.EnableDisablePlayer(false);
            _playerBehaviorControl.PlayPauseSpeech(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Pause(true);

        // Check what was the last camera
        if (_cameraSwitcher != null)
        {
            _lastCameraIndex = _cameraSwitcher.IsCurrentCameraPlayer();
            SwitchToRandomCam();
        }

        _pause.SetActive(true);
    }

    public void Continue()
    {
        SwitchToLastCam();

        _pause.SetActive(false);

        // Time.timeScale = 1;
        if (_holdingCamera != null)
            _holdingCamera.SetActive(true);

        if (_playerBehaviorControl != null)
        {
            _playerBehaviorControl.EnableDisablePlayer(true);
            _playerBehaviorControl.PlayPauseSpeech(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Pause(false);

        SetLastSetting();
    }

    private void SwitchToRandomCam()
    {
        if (_cameraSwitcher != null)
        {
            int ran = Random.Range(0, _cameraSwitcher.SecurityCameraAmount);
            _cameraSwitcher.SwitchSecurityCamera(ran);
        }
    }

    private void SwitchToLastCam()
    {
        if (_cameraSwitcher != null)
        {
            if (_lastCameraIndex == -1)
                _cameraSwitcher.SwitchToPlayerCamera();
            else
                _cameraSwitcher.SwitchSecurityCamera(_lastCameraIndex);
        }
    }

    private void SaveLastSetting()
    {
        for (int i = 0; i < _uiToCheck.Count; i++)
        {
            if (_uiToCheck[i] == null) continue;

            if (i >= _uiSetting.Count)
            {
                _uiSetting.Add(_uiToCheck[i].gameObject.activeSelf);
            }

            _uiSetting[i] = _uiToCheck[i].gameObject.activeSelf;
            _uiToCheck[i].gameObject.SetActive(false);
        }
    }

    private void SetLastSetting()
    {
        for (int i = 0; i < _uiToCheck.Count; i++)
        {
            if (_uiToCheck[i] == null) continue;

            if (i >= _uiSetting.Count)
            {
                _uiSetting.Add(_uiToCheck[i].gameObject.activeSelf);
            }

            _uiToCheck[i].gameObject.SetActive(_uiSetting[i]);
        }
    }

    public void AddRemoveUIToCheck(GameObject ui, bool addOrRemove)
    {
        if (ui == null) return;

        if (addOrRemove)
            _uiToCheck.Add(ui);
        else
            _uiToCheck.Remove(ui);
    }

    public void Pause(bool pauseOrNot)
    {
        if (_timeline != null)
        {
            if (pauseOrNot)
                _timeline.Pause();
            else
                _timeline.Play();
        }
    }
}
