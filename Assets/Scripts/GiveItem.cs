using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private GameObject _itemPrefab;

    public void GiveCoinsToPlayer()
    {
        Interactive itemInteractive = _itemPrefab.GetComponent<Interactive>();
        _playerInventory.Add(itemInteractive);
    }
}
