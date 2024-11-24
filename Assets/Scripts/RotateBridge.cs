using UnityEngine;

public class RotateBridge : MonoBehaviour
{
    [SerializeField] private RotateWhenHolding _rotate;
    private void OnEnable()
    {
        _rotate.enabled = false;
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
    }
}
