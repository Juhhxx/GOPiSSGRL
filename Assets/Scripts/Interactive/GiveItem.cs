using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    [SerializeField] private List<GameObject> _itemPrefab;

    private void Awake()
    {
        _playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    public void GiveItemToPlayer()
    {
        foreach (GameObject item in _itemPrefab)
        {
            Interactive itemInteractive = item.GetComponent<Interactive>();

            if (!_playerInventory.Contains(itemInteractive))
            {
                Debug.Log($"{gameObject.name} is giving item");
                _playerInventory.Add(itemInteractive);
            }
        }
    }
}
