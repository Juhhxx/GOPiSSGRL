using UnityEngine;
using System.Collections.Generic;

public class VerifySlushie : MonoBehaviour
{
    [SerializeField] private List<Flavours> _correcrCombination;
    private Interactive _interactive;
    private SlushieCup _slushieCup;
    private GiveItem _giveItem;
    private TakeItem _takeItem;

    private void Start()
    {
        _interactive = GetComponent<Interactive>();
        _slushieCup = FindAnyObjectByType<SlushieCup>();
        _giveItem = GetComponent<GiveItem>();
        _takeItem = GetComponent<TakeItem>();
    }
    public void Verify()
    {
        _slushieCup.ResetFlavours();
        _slushieCup.ResetPosition();

        if (!_slushieCup.CompareSlushies(_correcrCombination))
        {
            Debug.Log("NOT RIGTH!!");
            _interactive.ResetRequirements();
        }
        else
        {
            Debug.Log("YOU PASSED!");
            _giveItem.GiveItemToPlayer();
            _takeItem.TakeItemFromPlayer();
        }
    }
}
