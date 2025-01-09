using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private string _buttonOneScene;
    [SerializeField] private string _buttonTwoScene;

    public void ButtonOne()
    {
        SceneManager.LoadScene(_buttonOneScene);
    }
    public void ButtonTwo()
    {
        SceneManager.LoadScene(_buttonTwoScene);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
