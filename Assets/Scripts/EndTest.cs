using UnityEngine;

public class EndTest : MonoBehaviour
{
    [SerializeField] GameObject _finnishUI;

    public void ActivateUI()
    {
        _finnishUI.SetActive(true);
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
