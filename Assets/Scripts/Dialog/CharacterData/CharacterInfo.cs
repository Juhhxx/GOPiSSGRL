using UnityEngine;

[System.Serializable]
public class CharacterInfo
{
    [SerializeField] private CharacterID _characterID;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private string _characterName;

    public CharacterID CharacterID => _characterID;
    
    public AudioClip CharacterSound
    {
        get => _audioClip;
        set => _audioClip = value;
    }
    public string CharacterName
    {
        get => _characterName;
        set => _characterName = value;
    }
}
