using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _holdingCamera;
    private PlayerBehaviorControl _pbc;
    private void Awake()
    {
        _pbc = _player.GetComponent<PlayerBehaviorControl>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_pause.activeSelf)
            Pause();
        else if (Input.GetKeyDown(KeyCode.Escape))
            Continue();
    }
    private void CheckIfInteractiveHolding()
    {
        ViewBookUI scriptUI = _holdingCamera.GetComponentInChildren<ViewBookUI>();

        if (scriptUI != null)
            scriptUI.enabled = !scriptUI.enabled;
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        CheckIfInteractiveHolding();
        _pbc.EnableDisablePlayer(false);
        _pause.SetActive(true);
    }
    public void Continue()
    {
        _pause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        CheckIfInteractiveHolding();
        _pbc.EnableDisablePlayer(true);
    }
}
