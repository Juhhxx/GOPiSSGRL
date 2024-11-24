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
        _material = GetComponent<MeshRenderer>().material;
        _initialPos = transform.position;
    }
    private void Update()
    {
    }
    public void AddFlavour(Flavours flavour)
    {
        if (_slushieFlavours.Count < 4)
            _slushieFlavours.Add(flavour);
    }
    public void ResetFlavours()
    {
        _slushieFlavours.Clear();
    }
    public void ResetPosition()
    {
        transform.position = _initialPos;
    }
    public bool CompareSlushies(List<Flavours> ohterSlushie)
    {
        for (int i = 0; i < _slushieFlavours.Count; i++)
        {
            if (_slushieFlavours[i] != ohterSlushie[i])
                return false;
        }
        return true;
    }
    private void ChangeSlushieColor()
    {
        Color32 newColor = new Color(0,0,0,0);

        foreach (Flavours flavour in _slushieFlavours)
        {
            switch (flavour)
            {
                case Flavours.Red:
                    newColor += Color.red/4; break;
                case Flavours.Green:
                    newColor += Color.green/4; break;
                case Flavours.Blue:
                    newColor += Color.blue/4; break;
                case Flavours.Yellow:
                    newColor += Color.yellow/4; break;
            }
        }

        _material.color = newColor;
    }

}
