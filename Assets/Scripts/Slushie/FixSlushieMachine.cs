using UnityEngine;

public class FixSlushieMachine : MonoBehaviour
{
    [SerializeField] Interactive[] _slushieButtons;
    [SerializeField] float _correctTemprature = 32f;
    public void CheckTemprature(float value)
    {
        if (!(value <= _correctTemprature))
            foreach (Interactive button in _slushieButtons)
                if (button.RequirementsMet)
                    button.ResetRequirements();
    }
}
