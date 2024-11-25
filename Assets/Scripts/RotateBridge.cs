using Unity.VisualScripting;
using UnityEngine;

public class RotateBridge : MonoBehaviour
{
    [SerializeField] private RotateWhenHolding _rotate;
    [SerializeField] private bool _disableOnBegin;
    private FixSlushieMachine _fixMachine;

    private void Start()
    {
        _fixMachine = GetComponent<FixSlushieMachine>();
    }
    private void OnEnable()
    {
        if (!_disableOnBegin) EnableRotation();
        else DisableRotation();
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
        if (Input.GetButton("Use") || _rotate.enabled == false) return;
        _rotate.DisableRotation();
        _fixMachine.CheckTemprature();
    }
    public float GetCurrentValue()
    {
        return _rotate.GetCurrentValue();
    }
}
