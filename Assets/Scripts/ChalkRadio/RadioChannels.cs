using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class RadioChannels
{
    [field:SerializeField] public float Frequency { get; private set; }
    [field:SerializeField] public AudioClip Audio { get; private set; } 
}