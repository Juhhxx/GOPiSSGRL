using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Radio : MonoBehaviour
{
    [SerializeField] private float _frequency;
    private Transform _playerTrans;
    private AudioSource _audioSource;
    private ChalkDrawingPoint[] _chalkPoints;
    private List<float> _chalkFrequencies = new List<float>();

    private void Start()
    {
        _playerTrans = FindAnyObjectByType<PlayerMovement>().transform;
        _audioSource = GetComponent<AudioSource>();
        _chalkPoints = FindObjectsByType<ChalkDrawingPoint>(0);

        foreach (ChalkDrawingPoint point in _chalkPoints) _chalkFrequencies.Add(point.PointFrequency);
    }
    private void Update()
    {
        CheckFrequency();
    }
    private void CheckFrequency()
    {
        int pointIndex;

        if (_chalkFrequencies.Contains(_frequency))
        {
                pointIndex = _chalkFrequencies.IndexOf(_frequency);
                DetectDistance(pointIndex);
        }
        else
            Debug.Log("No poins in this frequency");
    }
    private void DetectDistance(int index)
    {
        Transform pointTrans = _chalkPoints[index].transform;
        float distance = Vector3.Distance(_playerTrans.position,pointTrans.position);

        ChangeAudioVolumeDistance(distance);

        Debug.Log($"Distance to player : {distance}");
    }  
    private void ChangeAudioVolumeDistance (float distance)
    {
        _audioSource.volume = 1 - distance/10;
    }
}