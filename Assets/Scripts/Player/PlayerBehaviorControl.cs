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
        if ( ! _playerInteraction.enabled ) return false;

        return _playerInteraction.CurrentInteractive == null;
    }

    public bool InventoryContains(Interactive requirement) => _playerInventory.Contains(requirement);

    public void ChangePlayerPosition(Vector3 position)
    {
        _playerMovement.transform.position =
            new Vector3(position.x, _playerMovement.transform.position.y, position.z);
    }
    public void PlayerLookAt(Vector3 position)
    {
        Vector3 direction = position - _playerMovement.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
             _playerMovement.transform.rotation = Quaternion.LookRotation(direction);
    }
}
