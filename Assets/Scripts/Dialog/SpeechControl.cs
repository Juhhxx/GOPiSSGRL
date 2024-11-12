using System.Collections;
using System.Collections.Generic;
using System.Text;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechControl : MonoBehaviour
{
    [SerializeField] private GameObject _dialogUI;
    [SerializeField] private TMP_Text _dialogText;
    [SerializeField] private RawImage _dialogBox;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private PlayerMovement _player;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField, MinMaxSlider(0f, 3f)] private Vector2 _pitchRange;

    [SerializeField] private CharacterInfoDatabase _characterInfo;
    // _characterdialogs defines a dialog stream for when interacting with each
    // character/object that needs dialog
    private Dictionary<CharacterID, Queue<(CharacterID, Queue<string>)>> _characterDialogs;

    private WaitForSeconds _waitForTypingSpeed;
    private StringBuilder _stringBuilder;
    private WaitUntil _waitUntilSpace;
    private WaitUntil _waitUntilSpaceOrDisplayed;
    private WaitForEndOfFrame _waitForEndOfFrame;
    private Coroutine _typingCoroutine;
    private IEnumerator _dialogCoroutine;
    private bool _isTextFullyDisplayed = false;

    // In start we just see if the text should be displayed based on if we have a
    // image box for the dialog,
    // and we initialize the waitforseconds that we will be using a lot, as well
    // as hide the ui
    private void Start()
    {
        _characterDialogs = new Dictionary<CharacterID, Queue<(CharacterID, Queue<string>)>>();

        _waitForTypingSpeed = new WaitForSeconds(_typingSpeed);
        _stringBuilder = new StringBuilder();

        _waitUntilSpace = new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        _waitUntilSpaceOrDisplayed = new WaitUntil(() => _isTextFullyDisplayed || Input.GetKeyDown(KeyCode.Space));
        _waitForEndOfFrame = new WaitForEndOfFrame();
        
        _dialogUI.SetActive(false);
    }

    // update is only here for testing
    #if UNITY_EDITOR
    private void Update()
    {
        // you can use the bool at the end of set character dialogs as false, to give the character
        // new dialog and keep it a dynamic enviornment, but at the same time, since you are not
        // reseting the characters dialog, they can always keep reminding the player on what to focus on
        // (banter stuff and all)
        // in resume: bool true for new player objective, no bool or bool false for random stuff
        if (Input.GetKeyDown(KeyCode.D))
        {
            Queue<string> clerkDialogs = new Queue<string>();
            clerkDialogs.Enqueue("Shut up");
            clerkDialogs.Enqueue("Buy this uv light right now.");
            clerkDialogs.Enqueue("Buy this uv light right now.");
            clerkDialogs.Enqueue("Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now.");
            SetCharacterDialogs(CharacterID.Clerk, clerkDialogs, CharacterID.Clerk);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Queue<string> clerkDialogs = new Queue<string>();
            clerkDialogs.Enqueue("ZZZZZZ");
            clerkDialogs.Enqueue("(Don't steal my stuff.)");
            clerkDialogs.Enqueue("ZZZZZzzz zzzzzzzzz zzzz zzzzzZZZZZZZ zzzz zzz zzZZZZZZZZZz zzzzzzz zz z");
            SetCharacterDialogs(CharacterID.Clerk, clerkDialogs, CharacterID.Clerk, true);
            clerkDialogs = new Queue<string>();
            clerkDialogs.Enqueue("psssssssssss");
            clerkDialogs.Enqueue("Im actually just pissing on you.");
            clerkDialogs.Enqueue("psssssssssss");
            SetCharacterDialogs(CharacterID.Clerk, clerkDialogs, CharacterID.Player);
            clerkDialogs = new Queue<string>();
            clerkDialogs.Enqueue("Kids these days...");
            SetCharacterDialogs(CharacterID.Clerk, clerkDialogs, CharacterID.Clerk);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShowDialog(CharacterID.Clerk);
        }
    }
    #endif

    // everytime the player advances in the puzzles, you can enqueue a queue
    // with a tuple of a character and its dialog, to be the value of the enum key
    // characterID, for when the player interacts with that character
    public void SetCharacterDialogs(CharacterID dialogID, Queue<string> dialogs, CharacterID characterID, bool resetCharacterQueue = false)
    {
        if (!_characterDialogs.ContainsKey(dialogID))
        {
            _characterDialogs.Add(dialogID, new Queue<(CharacterID, Queue<string>)>());
        }

        if (resetCharacterQueue)
            _characterDialogs[dialogID].Clear();
        
        (CharacterID, Queue<string>) tpl = (characterID, dialogs);

        _characterDialogs[dialogID].Enqueue(tpl);
    }

    // Show dialog gets a character ID and a name for the character, it checks if
    // the id exists and if so it starts the dialog, starting a coroutine that goes through
    // all the saved IEnumerators saved in dialogQueue
    public void ShowDialog(CharacterID dialogID)
    {
        if (!_characterDialogs.ContainsKey(dialogID) || _characterDialogs[dialogID].Count == 0)
        {
            Debug.Log("No dialog available for character ID: " + dialogID);
            return;
        }

        if (_dialogCoroutine != null)
            return;
        
        _inventoryUI.SetActive(false);
        // _player.enabled = false;
        _dialogUI.SetActive(true);

        _dialogCoroutine = ShowAllDialogs(dialogID);
        StartCoroutine(_dialogCoroutine);
    }

    // this method shows all the queued up dialogs from characters,
    // if you want to do a dialog that starts with player, goes to clerk and
    // back to player, you only need to set the dialogs in the correct order in setDialogs
    // and then use ShowDialog to set these dialogs in order for when they play together for each character
    private IEnumerator ShowAllDialogs(CharacterID dialogID)
    {
        Queue<(CharacterID, Queue<string>)> dialogQueue = _characterDialogs[dialogID];

        while(true)
        {
            yield return StartCoroutine(BeginDialog(dialogQueue.Peek()));

            if (dialogQueue.Count > 1)
            {
                dialogQueue.Dequeue();
                continue;
            }
            break;
        }

        EndDialog();
    }

    // This coroutine is called by showdialog and is used to go through all the
    // dialog lines in a characters dialog queue, every string is sent to begin typing,
    // if during the typing of these strings space is pressed, the dialog will just skip
    // to being complete, and only if space is pressed when its already complete,
    // it will go to the next dialog or stop displaying
    private IEnumerator BeginDialog((CharacterID, Queue<string>) dialogData)
    {
        string dialogToShow;

        CharacterID characterID = dialogData.Item1;
        Queue<string> dialogQueue = dialogData.Item2;

        string name = _characterInfo.GetName(characterID);
        AudioClip sound = _characterInfo.GetSound(characterID);

        while (true)
        {
            _isTextFullyDisplayed = false;

            ClearStringBuilder(name);

            _dialogText.text = _stringBuilder.ToString();
            dialogToShow = dialogQueue.Peek();

            _typingCoroutine = StartCoroutine(BeginTyping(dialogToShow, sound));

            yield return _waitUntilSpaceOrDisplayed;
            yield return _waitForEndOfFrame;

            if (!_isTextFullyDisplayed)
            {
                StopCoroutine(_typingCoroutine);

                ClearStringBuilder(name);

                _stringBuilder.Append(dialogToShow);
                _dialogText.text = _stringBuilder.ToString();
            }

            yield return _waitUntilSpace;
            yield return _waitForEndOfFrame;

            if (dialogQueue.Count > 1)
            {
                dialogQueue.Dequeue();
                continue;
            }
            break;
        }
    }

    // this method just clears the streamwriter that we are using in begindialog and
    // begin typing both, and adds a name if there is a name to add.
    // its only here to make code more clear
    private void ClearStringBuilder(string name)
    {
        _stringBuilder.Clear();
        if (!string.IsNullOrEmpty(name))
            _stringBuilder.Append(name).Append(": ");
    }

    // begin typing just receives a string which it will interpolate between all its
    // chars and add them to the stringbuilder and give that stringbuilder to the ui
    // text and then wait for typingspeed
    private IEnumerator BeginTyping(string dialogToShow, AudioClip sound)
    {
        foreach (char letter in dialogToShow)
        {
            _stringBuilder.Append(letter);
            _dialogText.text = _stringBuilder.ToString();

            if (IsNotAllowed(letter))
                // it waits double time on non letters
                yield return _waitForTypingSpeed;
            else if (sound != null)
            {
                // Debug.Log("Sound is not null and letter isnt white or space");
                _audioSource.pitch =
                    (float) UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y);
                _audioSource.PlayOneShot(sound);
                Debug.Log("Pitch is: " + _audioSource.pitch);
            }
            
            yield return _waitForTypingSpeed;
        }

        _isTextFullyDisplayed = true;
    }

    // A hashset of characters to not play a sound for in begin typing
    // it might be usefull to remove some of the symbols later if we want to
    // write censured bad words
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

    // End dialog disables all the necessary objects for the ui, and enables all the
    // ones that it had to disables when showDialog was first called
    private void EndDialog()
    {
        _dialogUI.SetActive(false);
        _inventoryUI.SetActive(true);
        // _player.enabled = true;

        _dialogCoroutine = null;
    }
}
