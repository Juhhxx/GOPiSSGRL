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
    [SerializeField] private MeshRenderer _necronomiconMeshR;
    [SerializeField] private Material _finalFrequencyMaterial;
    private Transform _playerTrans;
    private RotateBridge _rotateBridge;
    private AudioSource _audioSource;
    private List<ChalkDrawingPoint> _chalkPoints;
    private List<float> _chalkFrequencies = new List<float>();
    private ChalkDrawingPoint _finalPoint;
    private bool _allMarksDone;
    public bool AllMarksDone => _allMarksDone;

    private void Start()
    {
        _playerTrans = FindAnyObjectByType<PlayerMovement>().transform;
        _rotateBridge = GetComponent<RotateBridge>();
        _audioSource = GetComponent<AudioSource>();
        _chalkPoints = FindObjectsByType<ChalkDrawingPoint>(0).ToList<ChalkDrawingPoint>();

        _rotateBridge.EnableRotation();
        GetFinalPoint();

        foreach (ChalkDrawingPoint point in _chalkPoints) _chalkFrequencies.Add(point.PointFrequency);
    }
    private void Update()
    {
        // _frequency = _rotateBridge.GetCurrentValue();
        _frequencyDisplay.text = $"{_frequency:000} MHz";
        CheckFrequency();
        ShowLastFrequency();
        CheckPuzzleComplete();
    }
    // PASS TO SUMMON DEMON
    private void GetFinalPoint()
    {
        foreach (ChalkDrawingPoint point in _chalkPoints)
            if (point.GetComponent<TAG_FinalChalkPoint>() != null)
            {
                _finalPoint = point;
                _chalkPoints.Remove(_finalPoint);
                _finalPoint.gameObject.GetComponent<Collider>().enabled = false;
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
    // PASS TO SUMMON DEMON
    private bool CheckMarksDone()
    {
        foreach (ChalkDrawingPoint point in _chalkPoints)
        {
            if (!point.IsDrawn)
                return false;
        }
        return true;
    }
    // PASS TO SUMMON DEMON
    private void ShowLastFrequency()
    {
        if (CheckMarksDone())
        {
            _necronomiconMeshR.material = _finalFrequencyMaterial;
            _finalPoint.gameObject.GetComponent<Collider>().enabled = false;
        }
    }
    // PASS TO SUMMON DEMON
    private void CheckPuzzleComplete()
    {
        if (_finalPoint.IsDrawn) _allMarksDone = true; Debug.Log("ALL CHALK MARKS DONE");
    }
}