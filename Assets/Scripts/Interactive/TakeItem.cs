using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> _itemPrefabs; 

    private PlayerInventory _playerInventory;

    private void Awake()
    {
        _playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void TakeItemFromPlayer()
    {
        foreach (GameObject itemGmOb in _itemPrefabs)
        {
            Interactive item = itemGmOb.GetComponent<Interactive>();

            if (_playerInventory.Contains(item))
                _playerInventory.Remove(item);
        }
    }
    public bool TakeItemFromPlayer(Interactive item)
    {
        bool result = _playerInventory.IsSelected(item);

        if (result)
            _playerInventory.Remove(item);

        return result;
    }
}
