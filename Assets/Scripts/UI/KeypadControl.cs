using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadControl : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimator;
    [SerializeField] private GameObject _ui;
    [SerializeField] private InteractiveData _UVLightData;
    [SerializeField] private GameObject _keypadUI;
    [SerializeField] private TMP_Text _keypadText;
    [SerializeField] private RawImage _keypadGlitchImage;
    [SerializeField] private List<GameObject> _buttonsZeroToNine;
    [SerializeField] private int[] _sequence = new int[10] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    [SerializeField, Range(1, 10)] private int _combinationSize;
    [SerializeField] private List<int> _correctCombination;
    [SerializeField] private Material _uvMaterial;
    [SerializeField] private Texture[] _uvTextures;
    [SerializeField] private Interactive _requirement;
    private Stack<int> _currentCombination;
    private PlayerBehaviorControl _playerControl;

    // initializes needed collections and sets objects to their needed initial values
    // Generates _combinationSize x of non repeating numbers and orders them by
    // the order of sequence, themn gives them to correctCombination
    // It turns off the keypad UI after its finished initializing everything
    private void Awake()
    {
        Color color = _keypadGlitchImage.color;
        color.a = 0f;
        _keypadGlitchImage.color = color;

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

        int ran;
        for (int i = 0; i < _buttonsZeroToNine.Count; i++)
        {
            if (_correctCombination.Contains(i))
            {
                ran = Random.Range(0, _uvTextures.Length);


                Material _newUV = new Material(_uvMaterial);


                // Set the (uv texture) texture that should appear on th material when
                // the uv light is shined on it
                _newUV.SetTexture("_MainTex", _uvTextures[ran]);
                Renderer renderer = _buttonsZeroToNine[i].GetComponent<MeshRenderer>();

                renderer.material = _newUV;
            }
        }
    }
    private void Start()
    {
        _playerControl = FindFirstObjectByType<PlayerBehaviorControl>();

        _keypadUI.SetActive(false);
    }

    // TMP_text component isnt letting me set it directly on enable so i have an
    // Enumerator to tell me if TMP already exists.
    public void EnableKeypad()
    {
        _keypadUI.SetActive(true);

        _playerControl.EnableDisablePlayer(false);

        // _ui.SetActive(false);
        StartCoroutine(UpdateScreenIfText());
        Cursor.lockState = CursorLockMode.None;
    }
    private IEnumerator UpdateScreenIfText()
    {
        yield return new WaitUntil(() => _keypadText != null);
        UpdateScreen();
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
    private void Update()
    {
        if (Input.GetButtonDown("Exit") && _keypadUI.activeSelf && _playerControl.CanInteract())
            TurnOffKeypadUI();
    }

    // Turns off keypad UI when the player presses the X button
    public void TurnOffKeypadUI()
    {
        _playerControl.EnableDisablePlayer(true);

        // _ui.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        _keypadUI.SetActive(false);
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

        if (!_playerControl.InventoryContains(_requirement))
        {
            Debug.Log("Player not allowed to move on yet.");
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

    // This enumerator is only run after the player put the correct code in, it should
    // give a nice beepy sound and flash the code that they input.
    // Still to be tought if other methods in teh script should be disabled after this
    private IEnumerator CorrectCodeOpenDoor()
    {
        // Maybe send the confirmation of correctness back to the door and a nice correct beepy sound

        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        
        for (int i = 0; i <= 6 ; i++)
        {
            _keypadText.enabled = !_keypadText.enabled;
            yield return wfs;
        }
        
        _keypadText.enabled = true;

        _doorAnimator.SetTrigger("Open");
        TurnOffKeypadUI();
        enabled = false;
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

    // When there is an error in the keypad input, this method turns on the glitch material
    // that does some flashing and tv scratch stuff, and also flashes the keypadText
    // and sounds a beep sound for wrongness
    private IEnumerator KeypadError()
    {
        // Maybe add an error sound here?

        Color color =  _keypadGlitchImage.color;
        
        color.a = 1f;
        _keypadGlitchImage.color = color;

        float t = 0f;
        float flashTime = 0.6f;
        while ( t < flashTime)
        {
            t += Time.deltaTime;
            _keypadText.enabled = !_keypadText.enabled;
            yield return null;
        }

        color.a = 0f;
        _keypadGlitchImage.color = color;

        _keypadText.enabled = true;

        _currentCombination.Clear();
        UpdateScreen();
    }

    // this method just plays a beep sound for acceptfulness
    private void KeypadSuccessfulPress()
    {
        // Maybe a pressy beepy sound here to signal acceptableness
    }
}
