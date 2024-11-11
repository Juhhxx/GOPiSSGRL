using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechControl : MonoBehaviour
{
    [SerializeField] private GameObject _dialogUI;
    [SerializeField] private TMP_Text _dialogText;
    [SerializeField] private Image _dialogBox;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private PlayerMovement _player;

    private WaitForSeconds _waitForTypingSpeed;
    private StringBuilder _stringBuilder;
    private WaitUntil _waitUntilSpace;
    private WaitUntil _waitUntilNoSpace;
    private WaitUntil _waitUntilSpaceOrDisplayed;
    private WaitUntil _waitUntilNoSpaceOrDisplayed;
    private bool _dialogBoxMode = false;
    private Dictionary<CharacterID, Queue<string>> _characterDialogs = new Dictionary<CharacterID, Queue<string>>();
    private Coroutine _typingCoroutine;
    private Coroutine _dialogCoroutine;
    private bool _isTextFullyDisplayed = false;

    // In start we just see if the text should be displayed based on if we have a
    // image box for the dialog,
    // and we initialize the waitforseconds that we will be using a lot, as well
    // as hide the ui
    private void Start()
    {
        _dialogBoxMode = _dialogBox != null;

        _waitForTypingSpeed = new WaitForSeconds(_typingSpeed);
        _stringBuilder = new StringBuilder();
        _waitUntilSpace = new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        _waitUntilNoSpace = new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        _waitUntilSpaceOrDisplayed = new WaitUntil(() => _isTextFullyDisplayed || Input.GetKeyDown(KeyCode.Space));
        _waitUntilNoSpaceOrDisplayed = new WaitUntil(() => _isTextFullyDisplayed || Input.GetKeyUp(KeyCode.Space));

        _dialogUI.SetActive(false);

        Queue<string> playerDialogs = new Queue<string>();
        playerDialogs.Enqueue("I really need to piss");
        playerDialogs.Enqueue("Can I have the bathroom key?");
        playerDialogs.Enqueue("Im gonna piss on the floor nvm");
        playerDialogs.Enqueue("psssssssssssssssss");
        SetCharacterDialogs(CharacterID.Player, playerDialogs);

        Queue<string> clerkDialogs = new Queue<string>();
        clerkDialogs.Enqueue("Shut up");
        clerkDialogs.Enqueue("Buy this uv light right now.");
        clerkDialogs.Enqueue("Buy this uv light right now.");
        clerkDialogs.Enqueue("Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now. Buy this uv light right now.");
        SetCharacterDialogs(CharacterID.Clerk, clerkDialogs);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ShowDialog(CharacterID.Player);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowDialog(CharacterID.Clerk);
        }
    }

    // everytime the player advances in the puzzles, the current dialog for
    // characters should be updated if relevant. (only after anything else relevant
    // happens would they get a new set of dialogs, and only after running showdialog
    // for this character would the dialogs be displayed)
    public void SetCharacterDialogs(CharacterID characterID, Queue<string> dialogs)
    {
        if (!_characterDialogs.ContainsKey(characterID))
        {
            _characterDialogs.Add(characterID, new Queue<string>());
        }
        _characterDialogs[characterID] = dialogs;
    }

    // Show dialog gets a character ID and a name for the character, it checks if
    // the id exists and if so it starts the dialog, by diabling and enabling necessary parts
    // and starting the coroutine that actually shows the dialog, but only if the
    // coroutine isnt already running
    private void ShowDialog(CharacterID characterID, string characterName = null)
    {
        if (!_characterDialogs.ContainsKey(characterID) || _characterDialogs[characterID].Count == 0)
        {
            Debug.LogWarning("No dialog available for character ID: " + characterID);
            return;
        }

        if (_dialogCoroutine != null)
            return;
        
        _inventoryUI.SetActive(false);
        // _player.enabled = false;
        _dialogUI.SetActive(true);

        _dialogCoroutine = StartCoroutine(BeginDialog(characterID, characterName));
    }

    // This coroutine is called by showdialog and is used to go through all the
    // dialog lines in a characters dialog queue, every string is sent to begin typing,
    // if during the typing of these strings space is pressed, the dialog will just skip
    // to being complete, and only if space is pressed when its already complete,
    // it will go to the next dialog or stop displaying
    private IEnumerator BeginDialog(CharacterID ID, string name = null)
    {
        string dialogToShow;

        while (true)
        {
            _stringBuilder.Clear();
            _isTextFullyDisplayed = false;

            if (!string.IsNullOrEmpty(name))
                _stringBuilder.Append(name).Append(": ");

            _dialogText.text = _stringBuilder.ToString();

            dialogToShow = _characterDialogs[ID].Peek();

            _typingCoroutine = StartCoroutine(BeginTyping(dialogToShow));

            yield return _waitUntilSpaceOrDisplayed;
            yield return _waitUntilNoSpaceOrDisplayed;

            if (!_isTextFullyDisplayed)
            {
                StopCoroutine(_typingCoroutine);
                _stringBuilder.Clear();

                if (!string.IsNullOrEmpty(name))
                    _stringBuilder.Append(name).Append(": ");

                _stringBuilder.Append(dialogToShow);

                _dialogText.text = _stringBuilder.ToString();
            }

            yield return _waitUntilSpace;
            yield return _waitUntilNoSpace;

            if (_characterDialogs[ID].Count > 1)
                _characterDialogs[ID].Dequeue();
            else
                break;
        }

        EndDialog();
    }

    // begin typing just receives a string which it will interpolate between all its
    // chars and add them to the stringbuilder and give that stringbuilder to the ui
    // text and then wait for typingspeed
    private IEnumerator BeginTyping(string dialogToShow)
    {
        foreach (char letter in dialogToShow)
        {
            _stringBuilder.Append(letter);
            _dialogText.text = _stringBuilder.ToString();
            
            yield return _waitForTypingSpeed;
        }

        _isTextFullyDisplayed = true;
    }

    // End dialog disables all the necessary objects for the ui, and enables all the
    // ones that it had to disables when showDialog was first called
    private void EndDialog()
    {
        _dialogCoroutine = null;
        _dialogUI.SetActive(false);
        _inventoryUI.SetActive(true);
        // _player.enabled = true;
    }
}
