using System.Collections;
using UnityEngine;

public class PlayerBehaviorControl : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _interactionPanel;
    [SerializeField] private SpeechControl _speechControl;
    private int _disabledNumber = 0;

    public void EnableDisablePlayer(bool falseOrTrue)
    {

        if (falseOrTrue == true)
        {
            _disabledNumber -= 1;
        }
        
        // Debug.Log($"Diabled Player Times : {_disabledNumber}");

        if (_disabledNumber == 0)
        {
            Debug.Log("chnaged");
            _playerMovement.enabled = falseOrTrue;
            _playerInteraction.enabled = falseOrTrue;
            _playerInventory.enabled = falseOrTrue;
            _inventoryUI.SetActive(falseOrTrue);

            _playerInteraction.ClearCurrentInteractive();
            
            _disabledNumber = 0;
        }

        if (falseOrTrue == false)
        {
            _disabledNumber += 1;
        }

        // Debug.Log($"Diabled Player Times : {_disabledNumber}");
    }

    public bool CanInteract()
    {
        if ( ! _playerInteraction.enabled ) return false;

        return _playerInteraction.CurrentInteractive == null;
    }
    public bool CanInteractItems()
    {
        return _playerInteraction.CurrentInteractive == null;
    }

    public bool InventoryContains(Interactive requirement) => 
    _playerInventory.Contains(requirement);

    public bool InventoryContains(Interactive interactive, out int slot) =>
    _playerInventory.Contains(interactive,out slot);

    public bool InventoryIsSelected(Interactive interactive) =>
    _playerInventory.IsSelected(interactive);

    public void ChangePlayerPosition(Vector3 position)
    {
        transform.position =
            new Vector3(position.x, transform.position.y, position.z);
        Debug.Log("Changed player pos: " + transform.position);

        Debug.Log(position.x + " "+transform.position.y+ " " + position.z);
    }

    public void PlayerLookAt(Vector3 position, Vector3 dirPosition)
    {
        transform.position =
            new Vector3(position.x, transform.position.y, position.z);
        
        Debug.Log("pos: " + position);
        Debug.Log("Current player pos: " + transform.position);

        Vector3 directionY = dirPosition - transform.position;
        directionY.y = 0f;

        if (directionY.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(directionY);

        Vector3 headDirection = dirPosition - _playerMovement.Head.position;
        float headRotationX = Mathf.Atan2(headDirection.y, headDirection.z) * Mathf.Rad2Deg;
        headRotationX -= 180f;

        if (headDirection.sqrMagnitude > 0.001f)
            _playerMovement.Head.localRotation = Quaternion.Euler(headRotationX, 0f, 0f);
        
        Debug.Log("Current player pos: " + transform.position);
    }

    public void PlayPauseSpeech(bool playOrPause)
    {
        if (!playOrPause)
        {
            _speechControl.Paused = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
            _speechControl.Paused = true;

        Debug.Log("speech control pause is " + _speechControl.Paused);
    }
}