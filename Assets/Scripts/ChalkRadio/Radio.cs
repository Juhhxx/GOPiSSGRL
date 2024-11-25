using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class Radio : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private TextMeshPro _frequencyDisplay;
    [SerializeField] private AudioClip[] _radioAudios;
    private Transform _playerTrans;
    private RotateWhenHolding _rotateHolding;
    private AudioSource _audioSource;
    private SummonDemon _summonDemon;

    private void Start()
    {
        _playerTrans = FindAnyObjectByType<PlayerMovement>().transform;
        _rotateHolding = GetComponentInChildren<RotateWhenHolding>();
        _audioSource = GetComponent<AudioSource>();
        _summonDemon = FindAnyObjectByType<SummonDemon>();
    }
    private void Update()
    {
        _frequency = _rotateHolding.GetCurrentValue();
        _frequencyDisplay.text = $"{_frequency:f1} MHz";
        CheckFrequency();
    }
    
    private void CheckFrequency()
    {
        int pointIndex;
        float correctedFrequency = Mathf.Floor(_frequency);
        Debug.Log($"Current Frequency: {correctedFrequency}");
        if (_summonDemon.ChalkFrequencies.Contains(correctedFrequency))
        {
                pointIndex = _summonDemon.ChalkFrequencies.IndexOf(correctedFrequency);
                Debug.Log(pointIndex);
                DetectDistance(pointIndex);
                ChangeAudio(_radioAudios[1]);
        }
        else
        {
            ChangeAudio(_radioAudios[0]);
            ChangeAudioVolumeDistance(0f);
            Debug.Log("No poins in this frequency");
        }
    }
    private void DetectDistance(int index)
    {
        Transform pointTrans = _summonDemon.ChalkPoints[index].transform;

        // Corrects the position so the y value is ignored
        Vector3 correctedPoint = pointTrans.position;
        correctedPoint.y = 0f;

        Vector3 correctedPlayer = _playerTrans.position;
        correctedPlayer.y = 0f;

        float distance = Vector3.Distance(correctedPlayer,correctedPoint);

        ChangeAudioVolumeDistance(distance);

        Debug.Log($"Distance to player : {distance}");
    }  
    private void ChangeAudioVolumeDistance (float distance)
    {
        _audioSource.volume = Mathf.InverseLerp(12f,0.5f,distance);
    }
    private void ChangeAudio(AudioClip audio)
    {
        if (audio != _audioSource.clip)
        {
            _audioSource.clip = audio;
            _audioSource.Play();
        }
    }
    
}