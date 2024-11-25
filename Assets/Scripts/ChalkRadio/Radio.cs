using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class Radio : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private TextMeshPro _frequencyDisplay;
    [SerializeField] private MeshRenderer _necronomiconMeshR;
    [SerializeField] private Material _finalFrequencyMaterial;
    private Transform _playerTrans;
    private RotateBridge _rotateBridge;
    private AudioSource _audioSource;
    private ChalkDrawingPoint[] _chalkPoints;
    private List<float> _chalkFrequencies = new List<float>();
    private ChalkDrawingPoint _finalPoint;
    private bool _allMarksDone;
    public bool AllMarksDone => _allMarksDone;

    private void Start()
    {
        _playerTrans = FindAnyObjectByType<PlayerMovement>().transform;
        _rotateBridge = GetComponent<RotateBridge>();
        _audioSource = GetComponent<AudioSource>();
        _chalkPoints = FindObjectsByType<ChalkDrawingPoint>(0);

        _rotateBridge.EnableRotation();
        GetFinalPoint();

        foreach (ChalkDrawingPoint point in _chalkPoints) _chalkFrequencies.Add(point.PointFrequency);
    }
    private void Update()
    {
        _frequency = _rotateBridge.GetCurrentValue();
        _frequencyDisplay.text = $"{_frequency:000} MHz";
        CheckFrequency();
        ShowLastFrequency();
        CheckPuzzleComplete();
    }
    private void GetFinalPoint()
    {
        foreach (ChalkDrawingPoint point in _chalkPoints)
            if (point.GetComponent<TAG_FinalChalkPoint>() != null)
            {
                _finalPoint = point;
                _finalPoint.gameObject.SetActive(false);
                return;
            }
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
    private bool CheckMarksDone()
    {
        foreach (ChalkDrawingPoint point in _chalkPoints)
        {
            if (!point.IsDrawn)
                return false;
        }
        return true;
    }
    private void ShowLastFrequency()
    {
        if (CheckMarksDone())
        {
            _necronomiconMeshR.material = _finalFrequencyMaterial;
            _finalPoint.gameObject.SetActive(true);
            Debug.Log("ALL CHALK MARKS DONE");
        }
    }
    private void CheckPuzzleComplete()
    {
        if (_finalPoint.IsDrawn) _allMarksDone = true;
    }
}