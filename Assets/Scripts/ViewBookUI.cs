using UnityEngine;
using UnityEngine.UI;

public class ViewBookUI : MonoBehaviour
{
    [SerializeField] GameObject _viewUI;
    private Texture2D _bookTexture;
    private PlayerInteraction _playerInteraction;
    private PlayerBehaviorControl _playerControl;
    private GameObject _uiObject;

    private void Start()
    {
        _bookTexture = GetComponent<Renderer>().material.mainTexture as Texture2D;

        _playerControl = FindAnyObjectByType<PlayerBehaviorControl>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && _uiObject == null && 
        _playerInteraction.CurrentInteractive == null) 
            ShowUI();
        else if (Input.GetButtonDown("Exit") && _uiObject != null) HideUI();
    }

    private void ShowUI()
    {
        _uiObject = Instantiate(_viewUI);
        _uiObject.GetComponentInChildren<Button>().onClick.AddListener(HideUI);

        Image uiImage = _uiObject.GetComponentInChildren<Image>();
        Rect textureRect = new Rect(0f,0f,_bookTexture.width,_bookTexture.height);
        Sprite bookSprite = Sprite.Create(_bookTexture,textureRect,Vector2.zero);
        uiImage.sprite = bookSprite;

        Cursor.lockState = CursorLockMode.None;
        _playerControl.EnableDisablePlayer(true);
    }
    private void HideUI()
    {
        Destroy(_uiObject);
        _uiObject = null;
        Cursor.lockState = CursorLockMode.Locked;
        _playerControl.EnableDisablePlayer(false);
    }
}
