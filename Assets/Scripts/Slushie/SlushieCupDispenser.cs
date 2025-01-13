using UnityEngine;

public class SlushieCupDispenser : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private SlushieCup _slushieCup;
    [SerializeField] private AudioClip _returnSound;

    private TakeItem _takeItem;
    private Interactive _slushieInteractive;
    private Interactive _selfInteractive;
    private AudioSource _audioSource;
    private float _timesUsed = 0;
    
    private void Start()
    {
        _takeItem = GetComponent<TakeItem>();
        _slushieInteractive = _slushieCup.gameObject.GetComponent<Interactive>();
        _selfInteractive = GetComponent<Interactive>();

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.1f;
        _audioSource.clip = _returnSound;
    }
    public void GiveTakeSlushie()
    {
        if (_timesUsed % 2 == 0)
            GiveSlushie();
        else
            TakeSlushie();

        
        _timesUsed++;
    }
    private void GiveSlushie()
    {
        _slushieCup.GiveSlushie(_playerInventory);
        _selfInteractive.SetInteractionMessage("Return slushie cup");
    }
    private void TakeSlushie()
    {
        if (_playerInventory.Contains(_slushieInteractive))
        {
            _takeItem.TakeItemFromPlayer();
            _slushieCup.ResetSlushie();
            _audioSource.Play();
            _selfInteractive.SetInteractionMessage("Take slushie cup");
        }
    }
}
