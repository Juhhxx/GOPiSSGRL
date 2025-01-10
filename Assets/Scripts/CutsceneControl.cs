using System.Collections;
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

        StartSceneSwitch();

        _timeLine.Play();
    }

    private IEnumerator StartSceneSwitch()
    {
        Debug.Log("Started Scene Switch. ");

        float passedTime = 0f;
        bool onOrOff = false;

        while (passedTime < 0.14f)
        {
            _baseScene.SetActive(onOrOff);
            _demonScene.SetActive(onOrOff);

            onOrOff = !onOrOff;

            passedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.16f - passedTime);
        }

        _baseScene.SetActive(false);
        _demonScene.SetActive(true);
    }

    public void EndCutscene()
    {
        _cameraSwitcher.SwitchToPlayerCamera();
        _playerBehaviorControl.EnableDisablePlayer(true);
        _timeLine.Stop();
    }

    public void SummonDemon()
    {
        _demonObject.SetActive(true);
    }
}
