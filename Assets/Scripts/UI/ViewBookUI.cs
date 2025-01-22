using UnityEngine;
using UnityEngine.UI;

public class ViewBookUI : MonoBehaviour
{
    [SerializeField] GameObject _viewUI;
    [SerializeField] AudioClip _openSound;
    [SerializeField] AudioClip _closeSound;
    private Texture2D _bookTexture;
    private PlayerBehaviorControl _playerControl;
    private AudioSource _audioSource;
    private GameObject _uiObject;
    private PauseMenu _pause;

    private void OnEnable()
    {
        _pause = FindFirstObjectByType<PauseMenu>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _bookTexture = GetComponent<Renderer>().material.mainTexture as Texture2D;

        _playerControl = FindFirstObjectByType<PlayerBehaviorControl>();

        _uiObject = Instantiate(_viewUI);
        
        Canvas canvas = _uiObject.GetComponentInChildren<Canvas>();
        // canvas.worldCamera = FindFirstObjectByType<TagUICamera>().GetComponent<Camera>();
        // canvas.planeDistance = 1f;

        _pause.AddRemoveUIToCheck(_uiObject, true);

        Image uiImage = _uiObject.GetComponentInChildren<Image>();
        Rect textureRect = new Rect(0f,0f,_bookTexture.width,_bookTexture.height);
        Sprite bookSprite = Sprite.Create(_bookTexture,textureRect,Vector2.zero);
        uiImage.sprite = bookSprite;

        _uiObject.SetActive(false);
    }

    private void Update()
    {
        // if ( ! _playerControl.CanInteract() ) return;

        if (Input.GetButtonDown("Interact") && _playerControl.CanInteractItems())
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
        _playerControl.EnableDisablePlayer(false);
        _audioSource.clip = _openSound;
        _audioSource.Play();
    }
    private void HideUI()
    {
        _uiObject.SetActive(false);

        // Cursor.lockState = CursorLockMode.Locked;
        _playerControl.EnableDisablePlayer(true);
        _audioSource.clip = _closeSound;
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        if (_uiObject == null) return;

        _pause.AddRemoveUIToCheck(_uiObject.GetComponentInChildren<Canvas>().gameObject, false);

        if ( _uiObject.activeSelf )
        {
            // Cursor.lockState = CursorLockMode.Locked;
            _playerControl.EnableDisablePlayer(true);
        }

            Destroy(_uiObject);
    }
}
