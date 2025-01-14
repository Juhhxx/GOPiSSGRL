using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class SummonDemon : MonoBehaviour
{
    [SerializeField] private PlayerBehaviorControl _playerBehaviourControl;
    [SerializeField] private Interactive _necroInteractive;
    [SerializeField] private MeshRenderer _necronomiconMeshR;
    [SerializeField] private Material[] _necronomiconMaterials;
    [SerializeField] private GameObject _inventorySlots;
    [SerializeField] private GameObject _highLight;
    [SerializeField] private float _fadeSpeed;
    private List<ChalkDrawingPoint> _chalkPoints;
    public List<ChalkDrawingPoint> ChalkPoints => _chalkPoints;
    private List<float> _chalkFrequencies = new List<float>();
    public List<float> ChalkFrequencies => _chalkFrequencies;
    private ChalkDrawingPoint _finalPoint;
    private CutsceneControl _cutsceneContol;
    private YieldInstruction _wff;
    private bool _necroHold;

    private void Start()
    {
        _cutsceneContol = FindFirstObjectByType<CutsceneControl>();
        _wff = new WaitForEndOfFrame();
        _highLight.SetActive(false);

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
        _necroHold = _playerBehaviourControl.InventoryIsSelected(_necroInteractive);

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
            NecronomiconGlowControl();
            _finalPoint.gameObject.GetComponent<Collider>().enabled = true;
            _chalkFrequencies.Add(_finalPoint.PointFrequency);
            _chalkPoints.Add(_finalPoint);
        }
    }
    private void NecronomiconGlowControl()
    {
        if (_playerBehaviourControl.InventoryContains(_necroInteractive, out int slot))
        {
            GameObject necroSlot = _inventorySlots.transform.GetChild(slot).gameObject;
            RectTransform necroTrans = necroSlot.GetComponent<RectTransform>();
            RectTransform highLightTrans = _highLight.GetComponent<RectTransform>();

            highLightTrans.anchoredPosition = necroTrans.anchoredPosition;
            _highLight.SetActive(true);
            Image highLightImg = _highLight.GetComponent<Image>();

            StartCoroutine(NecronomiconGlow(highLightImg));
        }
    }
    private IEnumerator NecronomiconGlow(Image image)
    {
        Color newColor = Color.white;
        float newAlpha;
        float t = 0;

        while(!_necroHold)
        {
            Debug.Log("CAHNGING NECRO ALPHA");
            newAlpha = Mathf.Lerp( 1.0f, 0.0f, Mathf.PingPong( t, 1.0f));

            newColor.a = newAlpha;

            image.color = newColor;

            t += _fadeSpeed * Time.deltaTime;

            yield return _wff;
        }

        _highLight.SetActive(false);
    }
    private void CheckPuzzleComplete()
    {
        if (_finalPoint.IsDrawn) 
        {
            _cutsceneContol.AwakeDemon();
            gameObject.SetActive(false);
            _highLight.SetActive(false);
        }
    }
}
