using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float _volume;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayThisSound(AudioClip clip)
    {
        _audioSource.clip   = clip;
        _audioSource.volume = _volume;

        if (!_audioSource.isPlaying) _audioSource.Play();
    }
}
