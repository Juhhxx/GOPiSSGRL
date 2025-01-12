using UnityEngine;
using UnityEngine.Rendering;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _holdingCamera;
    [SerializeField] private SecurityCameraSwitcher _cameraSwitcher;
    [SerializeField] private PlayerBehaviorControl _playerBehaviorControl;
    private int _lastCameraIndex = -1;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_pause.activeSelf && !_settings.activeSelf)
            Pause();
        else if (Input.GetKeyDown(KeyCode.Escape))
            Continue();
    }
    private void CheckIfInteractiveHolding(bool set)
    {
        ViewBookUI scriptUI = _holdingCamera.GetComponentInChildren<ViewBookUI>();

        if (scriptUI != null)
            scriptUI.enabled = set;
    }
    public void Pause()
    {
        _playerBehaviorControl.PlayPauseSpeech(true);

        Cursor.lockState = CursorLockMode.None;
        // Time.timeScale = 0;
        CheckIfInteractiveHolding(false);
        _playerBehaviorControl.EnableDisablePlayer(false);
        CheckIfInteractiveHolding(false);


        // check what was teh last camera
        _lastCameraIndex = _cameraSwitcher.IsCurrentCameraPlayer();
        SwitchToRandomCam();


        _pause.SetActive(true);
    }
    public void Continue()
    {
        SwitchToLastCam();

        _pause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        // Time.timeScale = 1;
        CheckIfInteractiveHolding(true);
        _playerBehaviorControl.EnableDisablePlayer(true);
        _playerBehaviorControl.PlayPauseSpeech(false);
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
}
