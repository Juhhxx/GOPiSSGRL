using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class VerifySlushie : MonoBehaviour
{
    [SerializeField] private List<Flavours> _correctCombination;
    private Interactive _interactive;
    private SlushieCup _slushieCup;
    private GiveItem _giveItem;
    private TakeItem _takeItem;
    [SerializeField] private DialogBridge _correctSpeech;
    [SerializeField] private CharacterSpeech _wrongSpeech;

    private void Start()
    {
        _interactive = GetComponent<Interactive>();
        _slushieCup = FindAnyObjectByType<SlushieCup>();
        _giveItem = GetComponent<GiveItem>();
        _takeItem = GetComponent<TakeItem>();
    }
    public void Verify()
    {
        if (!_slushieCup.CompareSlushies(_correctCombination))
        {
            
            _wrongSpeech.StartSpeech();
            Debug.Log("NOT RIGTH!!");
            _interactive.ResetRequirements();
        }
        else
        {
            _correctSpeech.NextDialog();
            Debug.Log("YOU PASSED!");
            _giveItem.GiveItemToPlayer();
            _takeItem.TakeItemFromPlayer();
        }
        _slushieCup.ResetFlavours();
        _slushieCup.ResetPosition();
        _slushieCup.gameObject.SetActive(true);
    }
}
