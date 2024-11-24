using UnityEngine;

public class WarnIce : MonoBehaviour
{
    private MeltIce _ice;
    private void Awake()
    {
        _ice = FindFirstObjectByType<MeltIce>();

        if (_ice == null) Debug.Log("Ice not found. ");
    }
    public void WarnIceToMelt()
    {
        _ice.BeginCheckToMelt();
    }
}
