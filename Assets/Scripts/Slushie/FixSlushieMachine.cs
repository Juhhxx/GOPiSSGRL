using UnityEngine;

public class FixSlushieMachine : MonoBehaviour
{
    [SerializeField] Interactive[] _slushieButtons;
    [SerializeField] float _correctTemprature = 32f;
    private RotateBridge _thermostatRotate;

private void Start()
{
    _thermostatRotate = GetComponent<RotateBridge>();
}
    public void CheckTemprature()
    {
        if (!(_thermostatRotate.GetCurrentValue() <= _correctTemprature))
            foreach (Interactive button in _slushieButtons)
                if (button.RequirementsMet)
                    button.ResetRequirements();
    }
}
