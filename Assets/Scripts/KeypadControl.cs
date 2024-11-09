using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeypadControl : MonoBehaviour
{
    [SerializeField] private GameObject _keypadScreen;
    [SerializeField] private Material _glitchMaterial;
    private TMP_Text _keypadText;
    private RawImage _keypadImage;
    [SerializeField] private int[] _sequence = new int[10] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    [SerializeField, Range(1, 10)] private int _combinationSize;
    [SerializeField] private List<int> _correctCombination;
    private Stack<int> _currentCombination;

    // initializes needed collections
    // Generates _combinationSize x of non repeating numbers and orders them by
    // the order of sequence, themn gives them to correctCombination
    // It turns off the keypad UI after its finished initializing everything
    private void Start()
    {
        _keypadImage = _keypadScreen.GetComponent<RawImage>();
        _keypadText = _keypadScreen.GetComponentInChildren<TMP_Text>();
        _keypadImage.material = null;

        _currentCombination = new Stack<int>();

        HashSet<int> uniqueNumbers = new HashSet<int>();
        int ranNum;
        _correctCombination = new List<int>(_combinationSize);

        while (uniqueNumbers.Count < _combinationSize)
        {
            ranNum = Random.Range(0, 10);
            uniqueNumbers.Add(ranNum);
        }

        foreach (int num in _sequence)
        {
            if (uniqueNumbers.Contains(num))
            {
                _correctCombination.Add(num);
            }
        }

        // gameObject.SetActive(false);
    }

    // if current combination is bellow the desired size it will add the number
    // of the button that was pressed and update screen
    public void AddNumber(int num)
    {
        if (_currentCombination.Count < _combinationSize)
        {
            _currentCombination.Push(num);
            KeypadSuccessfulPress();
            UpdateScreen();
        }
        else
        {
            Debug.Log("Number spaces filled");
            StartCoroutine(KeypadError());
        }
    }

    // Checks if the currentCombination has anything and removes the first item
    // in the stack, after which it updates the screen
    public void Delete()
    {
        if (_currentCombination.Count > 0)
        {
            _currentCombination.Pop();
            KeypadSuccessfulPress();
            UpdateScreen();
        }
        else
        {
            StartCoroutine(KeypadError());
        }
    }

    // Turns off keypad UI when the player presses the X button
    public void TurnOffKeypadUI()
    {
        gameObject.SetActive(false);
    }

    // First, if the current combination is not complete, it returns
    // then it checks if each int inside it corresponds to the correct combination
    private List<int> _currentCombinationList;
    public void TryPassword()
    {
        if (_currentCombination.Count < _combinationSize)
        {
            Debug.Log("Not enough numbers");
            StartCoroutine(KeypadError());
            return;
        }
            
        _currentCombinationList = _currentCombination.Reverse().ToList();
        Debug.Log("count: " + _currentCombinationList.Count);
        for (int i = 0; i < _currentCombinationList.Count; i++)
        {
            Debug.Log("current: " + _currentCombinationList[i] + " correct: " + _correctCombination[i]);
            if (_currentCombinationList[i] != _correctCombination[i])
            {
                Debug.Log("Wrong pass");
                StartCoroutine(KeypadError());
                return;
            }
        }

        Debug.Log("Correct pass");
        StartCoroutine(CorrectCodeOpenDoor());
    }
    private IEnumerator CorrectCodeOpenDoor()
    {
        // Maybe send the confirmation of correctness back to the door and a nice correct beepy sound

        WaitForSeconds wfs = new WaitForSeconds(0.4f);
        
        for (int i = 0; i <= 6 ; i++)
        {
            _keypadText.enabled = !_keypadText.enabled;
            yield return wfs;
        }
        
        _keypadText.enabled = true;
    }

    // This method is called when the keypad is updated.
    // builds a new string with the contents of currentCombination, if the combination
    // is not complete, it adds some underscores so the player knows how many
    // numbers long the code is
    int currentCount;
    private void UpdateScreen()
    {
        StringBuilder screenText = new StringBuilder(_combinationSize);

        currentCount = _currentCombination.Count;

        for (int i = 0; i < _combinationSize; i++)
        {
            if (i < currentCount)
            {
                screenText.Append(_currentCombination.ElementAt(currentCount - i - 1));
            }
            else
            {
                screenText.Append("_");
            }

            if ( i < _combinationSize -1)
                screenText.Append(" ");
        }

        _keypadText.text = screenText.ToString();
    }

    private IEnumerator KeypadError()
    {
        // Maybe add an error sound here?

        _keypadImage.material = _glitchMaterial;

        yield return new WaitForSeconds(1.2f);

        _keypadImage.material = null;
    }
    private void KeypadSuccessfulPress()
    {
        // Maybe a pressy beepy sound here to signal acceptfulness
    }
}
