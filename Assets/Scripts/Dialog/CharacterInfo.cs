using UnityEngine;

[System.Serializable]
public class CharacterInfo
{
    [SerializeField] private CharacterID characterID;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private string characterName;

    public CharacterID CharacterID => characterID;
    public AudioClip CharacterSound
    {
        get => audioClip;
        set => audioClip = value;
    }
    public string CharacterName
    {
        get => characterName;
        set => characterName = value;
    }
}
