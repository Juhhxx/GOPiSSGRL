using UnityEngine;

public class SlushieCupDispenser : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private SlushieCup _slushieCup;

    public void GiveSlushie()
    {
        _slushieCup.GiveSlushie(_playerInventory);
    }
}
