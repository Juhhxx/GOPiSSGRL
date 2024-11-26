using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlushieCup : MonoBehaviour
{
    [SerializeField] private List<Flavours> _slushieFlavours = new List<Flavours>();
    private Material _material;
    private Vector3 _initialPos;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().sharedMaterial;
        _material.color = Color.white;
        _initialPos = transform.position;
    }
    private void Update()
    {
    }
    public void AddFlavour(Flavours flavour)
    {
        if (_slushieFlavours.Count < 4)
            _slushieFlavours.Add(flavour);
        ChangeSlushieColor();
    }
    public void ResetFlavours()
    {
        _slushieFlavours.Clear();
        ChangeSlushieColor();
    }
    public void ResetPosition()
    {
        transform.position = _initialPos;
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
    private void ChangeSlushieColor()
    {
        Color32 newColor = new Color(0,0,0,1);
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

        _material.color = newColor;
    }

}
