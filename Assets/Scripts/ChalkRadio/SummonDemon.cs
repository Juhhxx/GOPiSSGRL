using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SummonDemon : MonoBehaviour
{
    [SerializeField] private MeshRenderer _necronomiconMeshR;
    [SerializeField] private Material[] _necronomiconMaterials;
    private List<ChalkDrawingPoint> _chalkPoints;
    public List<ChalkDrawingPoint> ChalkPoints => _chalkPoints;
    private List<float> _chalkFrequencies = new List<float>();
    public List<float> ChalkFrequencies => _chalkFrequencies;
    private ChalkDrawingPoint _finalPoint;
    private CutsceneControl _cutsceneContol;

    private void Start()
    {
        _cutsceneContol = FindFirstObjectByType<CutsceneControl>();

        _necronomiconMeshR.material = _necronomiconMaterials[0];
        
        _chalkPoints = FindObjectsByType<ChalkDrawingPoint>(0).ToList<ChalkDrawingPoint>();

        GetFinalPoint();

        foreach (ChalkDrawingPoint point in _chalkPoints)
            _chalkFrequencies.Add(point.PointFrequency);
    }
    private void Update()
    {
        ShowLastFrequency();
        CheckPuzzleComplete();
    }
    private void GetFinalPoint()
    {
        foreach (ChalkDrawingPoint point in _chalkPoints)
            if (point.GetComponent<TagFinalChalkPoint>() != null)
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
            _necronomiconMeshR.material = _necronomiconMaterials[1];
            _finalPoint.gameObject.GetComponent<Collider>().enabled = true;
            _chalkFrequencies.Add(_finalPoint.PointFrequency);
            _chalkPoints.Add(_finalPoint);
        }
    }
    private void CheckPuzzleComplete()
    {
        if (_finalPoint.IsDrawn) 
        {
            _cutsceneContol.AwakeDemon();
            gameObject.SetActive(false);
        }
    }
}
