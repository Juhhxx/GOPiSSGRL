using UnityEngine;

public class EndTest : MonoBehaviour
{
    [SerializeField] private GameObject _finnishUI;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _holdingCamera;

    public void ActivateUI()
    {
        _holdingCamera.SetActive(false);
        _finnishUI.SetActive(true);
        _player.GetComponent<PlayerMovement>().enabled = false;
        _player.GetComponent<PlayerInteraction>().enabled = false;
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
