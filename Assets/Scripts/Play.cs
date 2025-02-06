using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void Start()
    {
        SceneManager.LoadScene("MainSceneTests");
    }
}
