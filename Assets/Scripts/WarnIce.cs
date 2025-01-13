using UnityEngine;

public class WarnIce : MonoBehaviour
{
    [SerializeField] private MeltIce _ice;
    private void Awake()
    {
        if (_ice == null) Debug.Log("Ice not found. ");
    }
    public void WarnIceToMelt()
    {
        _ice.BeginCheckToMelt();
    }
}
