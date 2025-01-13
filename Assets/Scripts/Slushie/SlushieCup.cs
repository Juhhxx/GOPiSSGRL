using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class SlushieCup : MonoBehaviour
{
    [SerializeField] private List<Flavours> _slushieFlavours = new List<Flavours>();
    [SerializeField] private GameObject _slushieObject;
    [SerializeField] private float _slushieUpdateSpeed = 0.01f;
    private Material _material;
    private Vector3 _initialPos;
    public bool IsUsed { get; private set; }
    private WaitForEndOfFrame wait = new WaitForEndOfFrame();
    Interactive _slushie;

    private void Start()
    {
        _material = _slushieObject.GetComponentInChildren<MeshRenderer>().material;
        _slushieObject.SetActive(false);
        _initialPos = transform.position;
        IsUsed = false;
        _slushie = GetComponent<Interactive>();
    }
    
    public void GiveSlushie(PlayerInventory inventory)
    {
        
        Debug.Log($"Is Slushie being used? {IsUsed}");
        if (!IsUsed)
        {
            IsUsed = true;
            inventory.Add(_slushie);
            _slushie.PlayPickUpSound();
        }
    }
    public void ResetSlushie()
    {
        IsUsed = false;
        ResetPosition();
        ResetFlavours();
        Debug.Log("Reseting Slushie");
    }
    private void ResetPosition()
    {
        transform.position = _initialPos;
    }
    private void ResetFlavours()
    {
        _slushieFlavours.Clear();
        ChangeSlushie();
    }
    public bool CompareSlushies(List<Flavours> ohterSlushie)
    {
        if (_slushieFlavours.Count != ohterSlushie.Count)
            return false;
        
        for (int i = 0; i < _slushieFlavours.Count; i++)
        {
            if (_slushieFlavours[i] != ohterSlushie[i])
                return false;
        }
        return true;
    }
    public void AddFlavour(Flavours flavour)
    {
        if (_slushieFlavours.Count < 4)
            _slushieFlavours.Add(flavour);
        ChangeSlushie();
    }
    private void ChangeSlushie()
    {
        if (_slushieFlavours.Count() == 0) 
        {
            _material.color = Color.white; 
            _slushieObject.SetActive(false);
            return;
        }
        
        Color32 newColor = new Color(0,0,0,200);
        float median = _slushieFlavours.Count();

        foreach (Flavours flavour in _slushieFlavours)
        {
            switch (flavour)
            {
                case Flavours.Red:
                    newColor += Color.red/median; break;
                case Flavours.Green:
                    newColor += Color.green/median; break;
                case Flavours.Blue:
                    newColor += Color.blue/median; break;
                case Flavours.Yellow:
                    newColor += Color.yellow/median; break;
            }
        }

        if (median == 1) _slushieObject.SetActive(true);

        StartCoroutine(ChangeSlushieColor(_material.color,newColor));
        StartCoroutine(ChangeSlushieScale(_slushieObject.transform.localScale.y, median));

    }
    private IEnumerator ChangeSlushieScale(float currentScale, float scale)
    {
        float newScale = currentScale;
        float i = 0;

        Debug.Log("CHANGING SCALE");
        while (newScale != scale)
        {
            newScale = Mathf.Lerp(currentScale,scale,i);

            Debug.Log($"{newScale} = {scale} ? {newScale == scale}");

            _slushieObject.transform.localScale = new Vector3(1f,newScale,1f);

            i += _slushieUpdateSpeed * Time.deltaTime;

            yield return wait;
        }
    }
    private IEnumerator ChangeSlushieColor(Color currentColor, Color color)
    {
        Color newColor = currentColor;
        float i = 0;

        Debug.Log("CHANGING COLOR");
        while (newColor != color)
        {
            newColor = Color.Lerp(currentColor,color,i);

            Debug.Log($"COLOR {newColor} = {color} ? {newColor == color}");

            _material.color = newColor;

            i += _slushieUpdateSpeed * Time.deltaTime;

            yield return wait;
        }
    }

}
