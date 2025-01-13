using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class VerifySlushie : MonoBehaviour
{
    [SerializeField] private List<Flavours> _correctCombination;
    [SerializeField] private Animator _mainAnimator;
    [SerializeField] private CutsceneControl _cutsceneControl;
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
            Debug.Log("NOT RIGHT!!");
            _interactive.ResetRequirements();
        }
        else
        {
            _correctSpeech.NextDialog();
            Debug.Log("YOU PASSED!");
            _giveItem.GiveItemToPlayer();
            _takeItem.TakeItemFromPlayer();
            StartCoroutine(CheckIfDoneSpeaking());
        }
        _slushieCup.ResetSlushie();
        _slushieCup.gameObject.SetActive(true);
    }

    private IEnumerator CheckIfDoneSpeaking()
    {
        SpeechControl _speech = FindAnyObjectByType<SpeechControl>();

        yield return new WaitUntil(() => _speech.ShowingSpeech());

        while (_speech.ShowingSpeech())
        {
            yield return null;
        }
        
        
        _cutsceneControl.UnSummonDemon();
        _mainAnimator.SetTrigger("UnSummon");
    }
}
