using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private UIManager  _uiManager;
    [SerializeField] private float      _maxInteractionDistance;

    private Transform   _cameraTransform;
    private Interactive _currentInteractive;
    private bool        _refreshCurrentInteractive;

    
    public Interactive CurrentInteractive => _currentInteractive;


    void Start()
    {
        _cameraTransform            = GetComponentInChildren<Camera>().transform;
        _currentInteractive         = null;
        _refreshCurrentInteractive  = false;
    }

    void Update()
    {
        UpdateCurrentInteractive();
        CheckForPlayerInteraction();
    }

    private void UpdateCurrentInteractive()
    {
        Debug.DrawRay(_cameraTransform.position,_cameraTransform.forward * _maxInteractionDistance,Color.blue);

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hitInfo, _maxInteractionDistance))
            CheckObjectForInteraction(hitInfo.collider);
        else if (_currentInteractive != null)
            ClearCurrentInteractive();

        Debug.DrawLine(_cameraTransform.position,hitInfo.point,Color.red);
        Debug.DrawRay(hitInfo.point,hitInfo.normal * 0.5f,Color.green);
    }

    private void CheckObjectForInteraction(Collider collider)
    {
        Interactive interactive = collider.GetComponent<Interactive>();

        if (interactive == null || !interactive.isOn)
        {
            if (_currentInteractive != null)
                ClearCurrentInteractive();
        }
        else if (interactive != _currentInteractive || _refreshCurrentInteractive)
            SetCurrentInteractive(interactive);
    }

    public void ClearCurrentInteractive()
    {
        // Debug.Log("nulled interactive");
        _currentInteractive = null;
        _uiManager.HideInteractionPanel();
    }

    private void SetCurrentInteractive(Interactive interactive)
    {
        // Debug.Log("set interactive");
        _currentInteractive         = interactive;
        _refreshCurrentInteractive  = false;

        string interactionMessage = interactive.GetInteractionMessage();

        if (interactionMessage != null && interactionMessage.Length > 0)
            _uiManager.ShowInteractionPanel(interactionMessage);
        else
            _uiManager.HideInteractionPanel();
    }

    private void CheckForPlayerInteraction()
    {
        if (Input.GetButtonDown("Interact") && _currentInteractive != null)
        {
            _currentInteractive.Interact();
            _refreshCurrentInteractive = true;
        }
        
    }

    public void RefreshCurrentInteractive()
    {
        _refreshCurrentInteractive = true;
    }
}
