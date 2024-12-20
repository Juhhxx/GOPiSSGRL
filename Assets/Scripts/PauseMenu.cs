using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _holdingCamera;
    private PlayerInteraction _playerInteraction;
    private PlayerMovement _playerMovement;
    private void Awake()
    {
        // _playerInteraction = _player.GetComponent<PlayerInteraction>();
        // _playerMovement = _player.GetComponent<PlayerMovement>();
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
        // _playerInteraction.enabled = false;
        // _playerMovement.enabled = false;
        _holdingCamera.SetActive(false);
        _pause.SetActive(true);
    }
    public void Continue()
    {
        _pause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        // _playerInteraction.enabled = true;
        // _playerMovement.enabled = true;
        _holdingCamera.SetActive(true);
    }
}
