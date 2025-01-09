using UnityEngine;

public class CutsceneControl : MonoBehaviour
{
    [SerializeField] private SecurityCameraSwitcher _cameraSwitcher;
    private SummonDemon _summonDemon;
    private GameObject _timeLine;
    private PlayerBehaviorControl _playerBehaviorControl;

    private void Start()
    {
        _summonDemon = FindFirstObjectByType<SummonDemon>();
    }

    public void AwakeDemon()
    {
        _playerBehaviorControl.EnableDisablePlayer(false);
        _cameraSwitcher.SwitchSecurityCamera(0);
        _timeLine.SetActive(true);
    }

    public void SummonDemon()
    {
        _summonDemon.AwakeDemon();
    }
}
