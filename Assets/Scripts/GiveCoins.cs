using System.Collections.Generic;
using UnityEngine;

public class GiveCoins : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private List<InteractiveData> _listInteractiveData;
    private Queue<InteractiveData> _coinsInteractiveData;

    private void Start() => _coinsInteractiveData = new Queue<InteractiveData>(_listInteractiveData);
    public void GiveCoinsToPlayer()
    {
        Interactive coinInteractive = _coinPrefab.GetComponent<Interactive>();
        coinInteractive.interactiveData = _coinsInteractiveData.Dequeue();

        _playerInventory.Add(coinInteractive);
        Debug.Log("Give Coin");
    }
}
