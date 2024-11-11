using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfoDatabase", menuName = "Scriptable Objects/CharacterInfo")]
public class CharacterInfoDatabase : ScriptableObject
{
    [SerializeField]
    private List<CharacterInfo> characterList = new List<CharacterInfo>();

    private Dictionary<CharacterID, AudioClip> audioDictionary;
    private Dictionary<CharacterID, string> nameDictionary;

    private void OnEnable()
    {
        audioDictionary = new Dictionary<CharacterID, AudioClip>();
        nameDictionary = new Dictionary<CharacterID, string>();

        foreach (CharacterInfo charac in characterList)
        {
            audioDictionary[charac.CharacterID] = charac.CharacterSound;
            nameDictionary[charac.CharacterID] = charac.CharacterName;
        }
    }

    public AudioClip GetSound(CharacterID characterID)
    {
        audioDictionary.TryGetValue(characterID, out AudioClip sound);
        return sound;
    }

    public string GetName(CharacterID characterID)
    {
        nameDictionary.TryGetValue(characterID, out string name);
        return name;
    }
}
