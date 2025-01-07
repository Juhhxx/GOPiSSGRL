using UnityEngine;

public class PlayerBehaviorControl : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerInventory _playerInventory;

    // _playerControl = FindAnyObjectByType<PlayerBehaviorControl>();

    // _playerControl.EnableDisablePlayer(true);

    public void EnableDisablePlayer(bool falseOrTrue)
    {
        _playerMovement.enabled = falseOrTrue;
        _playerInteraction.enabled = falseOrTrue;
    }
    public bool CanInteract()
    {
        if ( ! _playerInteraction.enabled ) return true;

        return _playerInteraction.CurrentInteractive == null;
    }

    public bool InventoryContains(Interactive requirement) => _playerInventory.Contains(requirement);
}
