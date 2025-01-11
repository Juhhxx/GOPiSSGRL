using Unity.VisualScripting;
using UnityEngine;

public class RotateBridge : MonoBehaviour
{
    [SerializeField] private RotateWhenHolding _rotate;
    [SerializeField] private bool _disableOnBegin;
    [SerializeField] private FixSlushieMachine _fixMachine;

    private void Awake()
    {
        _fixMachine = GetComponent<FixSlushieMachine>();
    }
    private void OnEnable()
    {
        if (!_disableOnBegin) EnableRotation();
        else _rotate.enabled = false;
    }
    public void EnableRotation()
    {
        _rotate.enabled = true;
        _rotate.EnableRotation();
    }
    private void Update()
    {
        DisableRotation();
    }
    private void DisableRotation()
    {
        if (!Input.GetButtonUp("Interact") || _rotate.enabled == false) return;
        Debug.Log($"disabling {_rotate.gameObject.name}");
        _fixMachine.CheckTemprature(GetCurrentValue());
        _rotate.DisableRotation();
    }
    public float GetCurrentValue()
    {
        Debug.Log("Current rotate bridge value: " + _rotate.GetCurrentValue());
        return _rotate.GetCurrentValue();
    }
}
