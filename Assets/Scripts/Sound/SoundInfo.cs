using System;
using UnityEngine;

[System.Serializable]
public struct SoundInfo
{
    [SerializeField] private SoundID _soundID;
    [SerializeField] private AudioClip _audioClip;

    public SoundID SoundID => _soundID;
    
    public AudioClip Sound{
        get => _audioClip;
        set => _audioClip = value;
    }
}
