using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneControl : MonoBehaviour
{
    [SerializeField] private SecurityCameraSwitcher _cameraSwitcher;
    [SerializeField] private LightingControl _lightingControl;
    [SerializeField] private GameObject _baseScene;
    [SerializeField] private LightingPresets _basePreset;
    [SerializeField] private GameObject _demonScene;
    [SerializeField] private LightingPresets _demonPreset;
    [SerializeField] private Transform _newPlayerPosition;
    [SerializeField] private Transform _newPlayerDirection;
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
        _playerBehaviorControl.PlayerLookAt(_newPlayerDirection.position);
        _cameraSwitcher.SwitchSecurityCamera(0, false);

        _timeLine.Play();

        StartCoroutine(StartSceneSwitch());

        ShakeSecurityCam(1.0f);
    }

    private IEnumerator StartSceneSwitch()
    {
        Debug.Log("Started Scene Switch. ");

        float passedTime = 0f;
        bool onOrOff = false;

        _lightingControl.ChangeLighting(_demonPreset);

        while (passedTime < 0.13f)
        {
            _baseScene.SetActive(onOrOff);
            _demonScene.SetActive(!onOrOff);

            onOrOff = !onOrOff;

            passedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.16f - passedTime);
        }

        _baseScene.SetActive(false);
        _demonScene.SetActive(true);
    }

    public void EndTimeline()
    {
        _timeLine.Stop();
    }

    public void EndCutscene()
    {
        _cameraSwitcher.SwitchToPlayerCamera();
        _playerBehaviorControl.EnableDisablePlayer(true);
    }

    public void SummonDemon()
    {
        ShakeSecurityCam(2.0f);
        _demonObject.SetActive(true);
    }

    private void ShakeSecurityCam(float time)
    {
        Shaker shaker = _cameraSwitcher.GetCurrenIndexCam()?.GetComponent<Shaker>();

        if (shaker != null)
            shaker.Shake(time, 15f);
    }
}
