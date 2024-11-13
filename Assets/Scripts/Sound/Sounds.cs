using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Scriptable Objects/Sounds")]
public class Sounds : ScriptableObject
{
    [SerializeField] private List<SoundInfo> _soundList = new List<SoundInfo>();
    private Dictionary<SoundID, AudioClip> _audioDictionary;

    private void OnEnable()
    {
        _audioDictionary = new Dictionary<SoundID, AudioClip>();
        
        foreach(SoundInfo listed in _soundList)
        {
            _audioDictionary[listed.SoundID] = listed.Sound;
        }
    }

    public AudioClip GetSound(SoundID id)
    {
        _audioDictionary.TryGetValue(id, out AudioClip sound);
        return sound;
    }
}
