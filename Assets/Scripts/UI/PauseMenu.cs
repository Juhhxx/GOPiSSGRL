using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _holdingCamera;
    private PlayerBehaviorControl _pbc;
    private GameObject _holdingObject;
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
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _holdingObject = _holdingCamera.transform.GetChild(0).gameObject;
        _holdingObject.SetActive(false);
        _pbc.EnableDisablePlayer(false);
        _pause.SetActive(true);
    }
    public void Continue()
    {
        _pause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _holdingObject.SetActive(true);
        _pbc.EnableDisablePlayer(true);
    }
}
