using UnityEngine;
using UnityEngine.UI;

public class ViewBookUI : MonoBehaviour
{
    [SerializeField] GameObject _viewUI;
    private Texture2D _bookTexture;
    private PlayerBehaviorControl _playerControl;
    private GameObject _uiObject;
    private PauseMenu _pause;

    private void OnEnable()
    {
        _pause = FindFirstObjectByType<PauseMenu>();
    }

    private void Start()
    {
        _bookTexture = GetComponent<Renderer>().material.mainTexture as Texture2D;

        _playerControl = FindAnyObjectByType<PlayerBehaviorControl>();

        _uiObject = Instantiate(_viewUI);
        _uiObject.GetComponentInChildren<Button>().onClick.AddListener(HideUI);

        _pause.AddRemoveUIToCheck(_uiObject.GetComponentInChildren<Canvas>(), true);

        Image uiImage = _uiObject.GetComponentInChildren<Image>();
        Rect textureRect = new Rect(0f,0f,_bookTexture.width,_bookTexture.height);
        Sprite bookSprite = Sprite.Create(_bookTexture,textureRect,Vector2.zero);
        uiImage.sprite = bookSprite;

        _uiObject.SetActive(false);
    }

    private void Update()
    {
        // if ( ! _playerControl.CanInteract() ) return;

        if (Input.GetButtonDown("Interact"))
        {
            if (!_uiObject.activeSelf)
                ShowUI();
            else
                HideUI();
        }

    }

    private void ShowUI()
    {
        _uiObject.SetActive(true);

        // Cursor.lockState = CursorLockMode.None;
        // _playerControl.EnableDisablePlayer(false);
    }
    private void HideUI()
    {
        _uiObject.SetActive(false);

        // Cursor.lockState = CursorLockMode.Locked;
        // _playerControl.EnableDisablePlayer(true);
    }

    private void OnDestroy()
    {
        _pause.AddRemoveUIToCheck(_uiObject.GetComponentInChildren<Canvas>(), false);

        /*if ( _uiObject.activeSelf )
        {
            Cursor.lockState = CursorLockMode.Locked;
            _playerControl.EnableDisablePlayer(true);
        }*/

        Destroy(_uiObject);
    }
}
