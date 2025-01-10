using UnityEngine;
using UnityEngine.Playables;

public class CutsceneControl : MonoBehaviour
{
    [SerializeField] private SecurityCameraSwitcher _cameraSwitcher;
    [SerializeField] private GameObject _baseScene;
    [SerializeField] private GameObject _demonScene;
    [SerializeField] private Transform _newPlayerPosition;
    [SerializeField] private GameObject _demonObject;
    [SerializeField] private PlayableDirector _timeLine;
    private PlayerBehaviorControl _playerBehaviorControl;

    private void Start()
    {
        _playerBehaviorControl = FindFirstObjectByType<PlayerBehaviorControl>();
        _demonObject.SetActive(false);
        _demonScene.SetActive(false);
    }

    public void AwakeDemon()
    {
        _playerBehaviorControl.EnableDisablePlayer(false);
        
        _playerBehaviorControl.ChangePlayerPosition(_newPlayerPosition.position);
        _playerBehaviorControl.PlayerLookAt(_demonObject.transform.position);
        _cameraSwitcher.SwitchSecurityCamera(0, false);
        _baseScene.SetActive(false);
        _demonScene.SetActive(true);
        _timeLine.Play();
    }

    public void EndCutscene()
    {

    }

    public void SummonDemon()
    {
        _demonObject.SetActive(true);
    }
}
