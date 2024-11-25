using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SummonDemon : MonoBehaviour
{
    [SerializeField] private MeshRenderer _necronomiconMeshR;
    [SerializeField] private Material _finalFrequencyMaterial;
    [SerializeField] private GameObject _demonObject;
    private List<ChalkDrawingPoint> _chalkPoints;
    public List<ChalkDrawingPoint> ChalkPoints => _chalkPoints;
    private List<float> _chalkFrequencies = new List<float>();
    public List<float> ChalkFrequencies => _chalkFrequencies;
    private ChalkDrawingPoint _finalPoint;
    private bool _allMarksDone = false;

    private void Start()
    {
        _chalkPoints = FindObjectsByType<ChalkDrawingPoint>(0).ToList<ChalkDrawingPoint>();

        GetFinalPoint();

        foreach (ChalkDrawingPoint point in _chalkPoints) _chalkFrequencies.Add(point.PointFrequency);
    }
    private void Update()
    {
        ShowLastFrequency();
        CheckPuzzleComplete();
    }
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
            _finalPoint.gameObject.GetComponent<Collider>().enabled = true;
        }
    }
    private void CheckPuzzleComplete()
    {
        if (_finalPoint.IsDrawn) 
        {
            _demonObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
