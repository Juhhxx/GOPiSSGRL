using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KeypadControl : MonoBehaviour
{
    [SerializeField] private TMP_Text _keypadScreen;
    [SerializeField] private int[] _sequence = new int[10] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    [SerializeField, Range(1, 10)] private int _combinationSize;
    [SerializeField] private List<int> _correctCombination;
    private Stack<int> _currentCombination;

    // initializes needed collections
    // Generates _combinationSize x of non repeating numbers and orders them by
    // the order of sequence, themn gives them to correctCombination
    private void Start()
    {
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
            KeypadError();
        }
    }

    // First, if the current combination is not complete, it returns
    // then it checks if each int inside it corresponds to the correct combination
    private List<int> _currentCombinationList;
    public void TryPassword()
    {
        if (_currentCombination.Count < _combinationSize)
        {
            Debug.Log("Not enough numbers");
            KeypadError();
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
                KeypadError();
                return;
            }
        }

        Debug.Log("Correct pass");
        // Maybe send the confirmation of correctness back to the door and a nice correct beepy sound
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
            KeypadError();
        }
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
        _keypadScreen.text = screenText.ToString();
    }

    private void KeypadError()
    {
        // Maybe add an error sound here... and some glitch effects?
    }
    private void KeypadSuccessfulPress()
    {
        // Maybe a pressy beepy sound here to signal acceptfulness
    }
}
