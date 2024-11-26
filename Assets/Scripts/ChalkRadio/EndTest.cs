using UnityEngine;

public class EndTest : MonoBehaviour
{
    [SerializeField] GameObject _finnishUI;
    [SerializeField] private GameObject _player;

    public void ActivateUI()
    {
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
