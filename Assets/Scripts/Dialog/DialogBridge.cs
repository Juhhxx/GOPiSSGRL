using UnityEngine;

public class DialogBridge : MonoBehaviour
{
    private CharacterSpeech _characterSpeech;

    private void Awake()
    {
        _characterSpeech = GetComponentInParent<CharacterSpeech>();
    }

    public void StartDialog() => _characterSpeech.StartSpeech();
    public void NextDialog() => _characterSpeech.NextSpeech();
    public void StopDialog() => _characterSpeech.StopSpeech();
}
