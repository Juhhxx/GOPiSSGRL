using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    [SerializeField] private List<GameObject> _itemPrefab;
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _playerInventory = FindFirstObjectByType<PlayerInventory>();

        _audioSource = GetComponent<AudioSource>();
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

                if (_audioSource == null) continue;
                _audioSource.clip = itemInteractive.interactiveData.pickUpSound;
                _audioSource.Play();
            }
        }
    }
}
