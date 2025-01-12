using System.Collections;
using System.Collections.Generic;
using System.Text;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SpeechControl : MonoBehaviour
{
    [SerializeField] private GameObject _dialogUI;
    [SerializeField] private TMP_Text _dialogText;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private GameObject _inventoryUI;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioMixer _audioMixer;
    private string _pitchID;
    [SerializeField, MinMaxSlider(0.5f, 2f)] private Vector2 _pitchRange;

    [SerializeField] private CharacterInfoDatabase _characterInfo;
    // _characterdialogs defines a dialog stream for when interacting with each
    // character/object that needs dialog

    private Queue<(CharacterID, Queue<string>)> _currentDialogs;

    private WaitForSeconds _waitForTypingSpeed;
    private StringBuilder _stringBuilder;
    private WaitUntil _waitUntil;
    private WaitUntil _waitUntilOrDisplayed;
    private YieldInstruction _waitForEndOfFrame;
    private Coroutine _typingCoroutine;
    private IEnumerator _dialogCoroutine;
    private bool _isTextFullyDisplayed = false;

    private PlayerBehaviorControl _playerControl;

    public bool Paused { get; set;} = false;
    private WaitUntil _waitUntilNotPaused;

    /// <summary>
    /// In start we just see if the text should be displayed based on if we have a
    /// image box for the dialog,
    /// and we initialize the waitforseconds that we will be using a lot, as well
    /// as hide the ui
    /// </summary>
    private void Start()
    {
        _characterInfo.ReadDictionaries();
        
        _playerControl = FindAnyObjectByType<PlayerBehaviorControl>();

        _pitchID = "PitchShifterPitch";

        _waitForTypingSpeed = new WaitForSeconds(_typingSpeed);
        _stringBuilder = new StringBuilder();

        _waitUntil = new WaitUntil(() => Input.GetButtonDown("Talk") );
        _waitUntilOrDisplayed = new WaitUntil(() => _isTextFullyDisplayed || Input.GetButtonDown("Talk"));
        _waitForEndOfFrame = new WaitForFixedUpdate();
        _waitUntilNotPaused = new WaitUntil(() => !Paused);
        
        _dialogUI.SetActive(false);
    }

    public bool ShowingSpeech()
    {
        return _dialogCoroutine != null;
    }

    /// <summary>
    /// Show dialog gets a character ID and a name for the character, it checks if
    /// the id exists and if so it starts the dialog, starting a coroutine that goes through
    /// all the saved IEnumerators saved in dialogQueue
    /// </summary>
    /// <param name="newDialogQueue"></param>
    /// <returns></returns>
    public Queue<(CharacterID, Queue<string>)> ShowDialogs(Queue<(CharacterID, Queue<string>)> newDialogQueue)
    {
        if (_dialogCoroutine != null)
            return null;
        
        _inventoryUI.SetActive(false);

        _playerControl.EnableDisablePlayer(false);

        _dialogUI.SetActive(true);

        _currentDialogs = newDialogQueue;

        _dialogCoroutine = ShowAllDialogs();
        StartCoroutine(_dialogCoroutine);

        if (_currentDialogs != newDialogQueue)
            return _currentDialogs;
        else
            return null;
    }

    /// <summary>
    /// this method shows all the queued up dialogs from characters,
    /// if you want to do a dialog that starts with player, goes to clerk and
    /// back to player, you only need to set the dialogs in the correct order in setDialogs
    /// and then use ShowDialog to set these dialogs in order for when they play together for each character
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowAllDialogs()
    {
        _dialogText.text = null;
        yield return _waitForEndOfFrame;

        while(true)
        {
            // dont play if pause menu is on
            yield return _waitUntilNotPaused;

            yield return StartCoroutine(BeginDialog(_currentDialogs.Peek()));

            if (_currentDialogs.Count > 1)
            {
                _currentDialogs.Dequeue();
                continue;
            }
            break;
        }

        EndDialog();
    }

    /// <summary>
    /// This coroutine is called by showdialog and is used to go through all the
    /// dialog lines in a characters dialog queue, every string is sent to begin typing,
    /// if during the typing of these strings Talk is pressed, the dialog will just skip
    /// to being complete, and only if Talk is pressed when its already complete,
    /// it will go to the next dialog or stop displaying
    /// </summary>
    /// <param name="dialogData"></param>
    /// <returns></returns>
    private IEnumerator BeginDialog((CharacterID, Queue<string>) dialogData)
    {
        string dialogToShow;

        CharacterID characterID = dialogData.Item1;
        Queue<string> dialogQueue = dialogData.Item2;

        string name = _characterInfo.GetName(characterID);
        AudioClip sound = _characterInfo.GetSound(characterID);

        while (true)
        {
            // dont play if pause menu is on
            yield return _waitUntilNotPaused;
            yield return _waitForEndOfFrame;

            _isTextFullyDisplayed = false;

            ClearStringBuilder(name);

            _dialogText.text = _stringBuilder.ToString();
            dialogToShow = dialogQueue.Peek();

            _typingCoroutine = StartCoroutine(BeginTyping(dialogToShow, sound));

            yield return _waitUntilOrDisplayed;
            yield return _waitForEndOfFrame;

            if (!_isTextFullyDisplayed)
            {
                StopCoroutine(_typingCoroutine);

                ClearStringBuilder(name);

                _stringBuilder.Append(dialogToShow);
                _dialogText.text = _stringBuilder.ToString();
            }

            yield return _waitUntilNotPaused;
            yield return _waitForEndOfFrame;

            yield return _waitUntil;
            yield return _waitForEndOfFrame;

            if (dialogQueue.Count > 1)
            {
                dialogQueue.Dequeue();
                continue;
            }
            break;
        }
    }

    /// <summary>
    ///This method just clears the streamwriter that we are using in begindialog and
    /// begin typing both, and adds a name if there is a name to add.
    /// its only here to make code more clear
    /// </summary>
    /// <param name="name"></param>
    private void ClearStringBuilder(string name)
    {
        _stringBuilder.Clear();
        if (!string.IsNullOrEmpty(name))
            _stringBuilder.Append(name).Append(": ");
    }

    /// <summary>
    ///begin typing just receives a string which it will interpolate between all its
    /// chars and add them to the stringbuilder and give that stringbuilder to the ui
    /// text and then wait for typingspeed
    /// </summary>
    /// <param name="dialogToShow"></param>
    /// <param name="sound"></param>
    /// <returns></returns>
    private IEnumerator BeginTyping(string dialogToShow, AudioClip sound)
    {
        foreach (char letter in dialogToShow)
        {
            // dont play if pause menu is on
            yield return _waitUntilNotPaused;

            _stringBuilder.Append(letter);
            _dialogText.text = _stringBuilder.ToString();

            if (letter == '\\')
                continue;

            if (IsNotAllowed(letter))
                // it waits double time on non letters
                yield return _waitForTypingSpeed;
            else if (sound != null)
            {
                // Debug.Log("Sound is not null and letter isnt white or pressed talk");
                _audioMixer.SetFloat(_pitchID,
                    (float) UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y));
                _audioSource.PlayOneShot(sound);
                /*float f;
                _audioMixer.GetFloat(_pitchID, out f);
                Debug.Log("Pitch is: " + f);*/
            }

            yield return _waitForTypingSpeed;
        }

        _isTextFullyDisplayed = true;
    }

    /// <summary>
    ///A hashset of characters to not play a sound for in begin typing
    /// it might be usefull to remove some of the symbols later if we want to
    /// write censured bad words
    /// </summary>
    private readonly HashSet<char> notAllowedChars = new HashSet<char>()
    {
        '+', '=', '/', '\\', '|', '<', '>', '_', '^', '~', '"', '\'', '`','¢',
        '£', '€', '¥', '{', '}', '[', ']', '(', ')', '\0', '\n', '\t', '␣', ' ',
        '.', ',', ';', ':', '!', '?', '-', '@', '#', '*', '%', '&', '~', '…'
    };

    private bool IsNotAllowed(char c)
    {
        return notAllowedChars.Contains(c);
    }

    /// <summary>
    /// End dialog disables all the necessary objects for the ui, and enables all the
    /// ones that it had to disables when showDialog was first called
    /// </summary>
    private void EndDialog()
    {
        _dialogUI.SetActive(false);
        _inventoryUI.SetActive(true);

        _playerControl.EnableDisablePlayer(true);

        _dialogCoroutine = null;
    }
}
