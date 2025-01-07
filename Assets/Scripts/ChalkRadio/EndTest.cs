using UnityEngine;

public class EndTest : MonoBehaviour
{
    [SerializeField] private GameObject _finnishUI;
    [SerializeField] private PlayerBehaviorControl _playerControl;
    [SerializeField] private GameObject _holdingCamera;

    public void ActivateUI()
    {
        _holdingCamera.SetActive(false);
        _finnishUI.SetActive(true);

        _playerControl = FindAnyObjectByType<PlayerBehaviorControl>();
        _playerControl.EnableDisablePlayer(false);

        Cursor.lockState = CursorLockMode.None;
    }
    public void OpenForm()
    {
        Application.OpenURL("https://forms.gle/wNbjW4ipwmmDH9Rk8");
        Application.Quit();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
