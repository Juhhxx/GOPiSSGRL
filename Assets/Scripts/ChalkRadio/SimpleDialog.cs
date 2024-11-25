using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class SimpleDialog : MonoBehaviour
{
    [SerializeField] private List<DialogSentence> _dialogList;
    [SerializeField] private float _textSpeed;
    [SerializeField] private TextMeshProUGUI _tmp;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _interactionPanel;
    private PlayerMovement _playerMovement;
    private PlayerInteraction _playerInteraction;
    private Animator _anim;
    private Queue<DialogSentence> _dialogQueue;
    private DialogSentence _currentDialog;
    private DialogSentence _lastDialog;
    private YieldInstruction _textWait;
    private bool _isActive = false;
    void Start()
    {
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        _playerInteraction = FindAnyObjectByType<PlayerInteraction>();
        _anim = GetComponent<Animator>();
        _textWait = new WaitForSeconds(_textSpeed);
        _dialogQueue = new Queue<DialogSentence>();
    }
    void Update()
    {
        if (_isActive == true)
        {
            _tmp.text = DisplayDialog(_lastDialog);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_lastDialog.dialogSentence != _currentDialog.dialogSentence)
                    DisplaySentenceAtOnce();
                else if (_dialogQueue.Count != 0)
                    CreateNextSentence();
                else
                    EndDialog();
            }
        }
    }
    public void StartDialog ()
    {
        _dialogQueue.Clear();
        _dialogQueue = new Queue<DialogSentence>(_dialogList);
        _inventoryPanel.SetActive(false);
        _interactionPanel.SetActive(false);
        _playerMovement.enabled = false;
        _playerInteraction.enabled = false;
        _tmp.text = "";
        _tmp.enabled = true;
        _isActive = true;
        
        CreateNextSentence();
    }
    public void CreateNextSentence()
    {
        _currentDialog = _dialogQueue.Dequeue();
        _lastDialog.dialogSentence = "";
        
        StartCoroutine(TypeSentence(_currentDialog,_textSpeed));
    }
    private IEnumerator TypeSentence(DialogSentence print, float time)
    {
        _lastDialog.dialogName = $"{print.dialogName}";
        foreach (char c in print.dialogSentence.ToCharArray())
        {
            _lastDialog.dialogSentence += c;
            yield return _textWait;
        }
    }
    private void DisplaySentenceAtOnce()
    {
        StopAllCoroutines();
        _lastDialog = _currentDialog;
    }
    private string DisplayDialog(DialogSentence dialog)
    {
        return String.Format("{0} : {1}", dialog.dialogName, dialog.dialogSentence);
    }
    private void EndDialog()
    {
        _inventoryPanel.SetActive(true);
        _interactionPanel.SetActive(true);
        _playerMovement.enabled = true;
        _playerInteraction.enabled = true;
        _tmp.enabled = false;
        _isActive = false;
        _anim.SetTrigger("EndDialog");
    }
}
