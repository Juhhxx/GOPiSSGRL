using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfoDatabase", menuName = "Scriptable Objects/CharacterInfo")]
public class CharacterInfoDatabase : ScriptableObject
{
    [SerializeField]
    private List<CharacterInfo> characterList = new List<CharacterInfo>();

    private Dictionary<CharacterID, AudioClip> _audioDictionary;
    private Dictionary<CharacterID, string> _nameDictionary;

    private void OnEnable()
    {
        _audioDictionary = new Dictionary<CharacterID, AudioClip>();
        _nameDictionary = new Dictionary<CharacterID, string>();

        foreach (CharacterInfo charac in characterList)
        {
            _audioDictionary[charac.CharacterID] = charac.CharacterSound;
            _nameDictionary[charac.CharacterID] = charac.CharacterName;
        }
    }

    public AudioClip GetSound(CharacterID characterID)
    {
        _audioDictionary.TryGetValue(characterID, out AudioClip sound);
        return sound;
    }

    public string GetName(CharacterID characterID)
    {
        _nameDictionary.TryGetValue(characterID, out string name);
        return name;
    }
}
