using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private List<GameObject> _itemPrefab;

    public void GiveItemToPlayer()
    {
        foreach (GameObject item in _itemPrefab)
        {
            Debug.Log($"{gameObject.name} is giving item");
            Interactive itemInteractive = item.GetComponent<Interactive>();
            _playerInventory.Add(itemInteractive);
        }
    }
}
