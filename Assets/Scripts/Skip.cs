using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour
{
    private bool anyKeyWasPressed = false;
    [SerializeField] private string _string;
    [SerializeField] private TMP_Text _text;
    private bool _done = false;

    private void Start()
    {
        StartCoroutine(StringyString());
    }
    private IEnumerator StringyString()
    {
        _string.Reverse();

        foreach (char letter in _string)
        {
            _text.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        _done = true;
    }

    private void Update()
    {
        if ( ! _done) return;

        if (Input.anyKey)
            anyKeyWasPressed = true;

        // up input instead of down
        if (!Input.anyKey && anyKeyWasPressed)
            ChangeScene();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("MainSceneTests");
    }
}
