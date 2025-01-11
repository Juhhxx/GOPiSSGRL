using UnityEngine;

public class PlayerBehaviorControl : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerInventory _playerInventory;
    private int _disabledNumber = 0;

    public void EnableDisablePlayer(bool falseOrTrue)
    {
        if (falseOrTrue == true)
        {
            _disabledNumber -= 1;
        }
        
        Debug.Log($"Diabled Player Times : {_disabledNumber}");

        if (_disabledNumber == 0)
        {
            _playerMovement.enabled = falseOrTrue;
            _playerInteraction.enabled = falseOrTrue;
            _disabledNumber = 0;
        }

        if (falseOrTrue == false)
        {
            _disabledNumber += 1;
        }

        Debug.Log($"Diabled Player Times : {_disabledNumber}");

    }

    public bool CanInteract()
    {
        if ( ! _playerInteraction.enabled ) return false;

        return _playerInteraction.CurrentInteractive == null;
    }

    public bool InventoryContains(Interactive requirement) => _playerInventory.Contains(requirement);

    public void ChangePlayerPosition(Vector3 position)
    {
        transform.position =
            new Vector3(position.x, transform.position.y, position.z);
    }

    public void PlayerLookAt(Vector3 position)
    {
        Vector3 directionY = position - transform.position;
        directionY.y = 0f;

        if (directionY.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(directionY);

        Vector3 headDirection = position - _playerMovement.Head.position;
        float headRotationX = Mathf.Atan2(headDirection.y, headDirection.z) * Mathf.Rad2Deg;
        headRotationX -= 180f;

        if (headDirection.sqrMagnitude > 0.001f)
            _playerMovement.Head.localRotation = Quaternion.Euler(headRotationX, 0f, 0f);
    }
}