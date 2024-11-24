using UnityEngine;

public class DialogBridge : MonoBehaviour
{
    [SerializeField] private CharacterSpeech _characterSpeech;

    private void Awake()
    {
        if (_characterSpeech == null)
            _characterSpeech = GetComponentInParent<CharacterSpeech>();
    }

    public void StartDialog() => _characterSpeech.StartSpeech();
    public void NextDialog() => _characterSpeech.NextSpeech();
    public void StopDialog() => _characterSpeech.StopSpeech();
}
