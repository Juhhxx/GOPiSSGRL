using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class Radio : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private TextMeshPro _frequencyDisplay;
    private Transform _playerTrans;
    private RotateBridge _rotateBridge;
    private AudioSource _audioSource;
    private SummonDemon _summonDemon;

    private void Start()
    {
        _playerTrans = FindAnyObjectByType<PlayerMovement>().transform;
        _rotateBridge = GetComponent<RotateBridge>();
        _audioSource = GetComponent<AudioSource>();
        _summonDemon = FindAnyObjectByType<SummonDemon>();

        _rotateBridge.EnableRotation();
    }
    private void Update()
    {
        // _frequency = _rotateBridge.GetCurrentValue();
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
        }
        else
            Debug.Log("No poins in this frequency");
    }
    private void DetectDistance(int index)
    {
        Transform pointTrans = _summonDemon.ChalkPoints[index].transform;
        float distance = Vector3.Distance(_playerTrans.position,pointTrans.position);

        ChangeAudioVolumeDistance(distance);

        Debug.Log($"Distance to player : {distance}");
    }  
    private void ChangeAudioVolumeDistance (float distance)
    {
        _audioSource.volume = 1 - distance/10;
    }
    
}