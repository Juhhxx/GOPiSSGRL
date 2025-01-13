using UnityEngine;

public class SlushieCupDispenser : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private SlushieCup _slushieCup;
    [SerializeField] private AudioClip _pickSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.01f;
    }
    public void GiveSlushie()
    {
        _slushieCup.GiveSlushie(_playerInventory);
    }
}
