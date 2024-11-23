using UnityEngine;

public class DialogBridge : MonoBehaviour
{
    private CharacterSpeech _characterSpeech;

    private void Awake()
    {
        _characterSpeech = GetComponentInParent<CharacterSpeech>();
    }

    public void StartDialog()
    {
        _characterSpeech.StartDialog();
    }
    public void NextDialog()
    {
        _characterSpeech.NextDialog();
    }
}
