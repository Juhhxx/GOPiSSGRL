using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class CutsceneControl : MonoBehaviour
{
    [SerializeField] private SecurityCameraSwitcher _cameraSwitcher;
    [SerializeField] private Animator _animator;
    [SerializeField] private LightingControl _lightingControl;

    [SerializeField] private GameObject _baseScene;
    [SerializeField] private LightingPresets _basePreset;

    [SerializeField] private GameObject _demonScene;
    [SerializeField] private LightingPresets _demonPreset;

    [SerializeField] private GameObject _postScene;
    [SerializeField] private LightingPresets _postPreset;

    [SerializeField] private GameObject _endScene;
    [SerializeField] private LightingPresets _endPreset;

    [SerializeField] private Transform _newPlayerPosition;
    [SerializeField] private Transform _newPlayerDirection;

    [SerializeField] private Transform _endPlayerPosition;

    [SerializeField] private GameObject _demonObject;

    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private TimelineAsset _demonTimeline;
    [SerializeField] private TimelineAsset _endTimeline;


    [SerializeField] private Animator _pissyMissy;
    private PlayerBehaviorControl _playerBehaviorControl;

    [SerializeField] private GameObject _finnishUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject[] _disablePlayerView;


    private void Awake()
    {
        _timeline.playableAsset = null;
    }
    private void Start()
    {

        _playerBehaviorControl = FindFirstObjectByType<PlayerBehaviorControl>();
        _demonObject.SetActive(false);
        _demonScene.SetActive(false);
        _postScene.SetActive(false);
        _endScene.SetActive(false);

        _baseScene.SetActive(true);

        _cameraSwitcher.SwitchSecurityCamera(2, false);
        _cameraSwitcher.SwitchToPlayerCamera();

        _lightingControl.ChangeLighting(_basePreset);
    }

    public void AwakeDemon()
    {
        Debug.Log("pt pos: " + _newPlayerPosition.position);
        _playerBehaviorControl.PlayerLookAt(_newPlayerPosition.position, _newPlayerDirection.position);

        _playerBehaviorControl.EnableDisablePlayer(false);
        _cameraSwitcher.SwitchSecurityCamera(0, false);

        _timeline.playableAsset = _demonTimeline;
        _timeline.Play();

        StartCoroutine(StartSceneSwitch(_baseScene, _demonPreset, _demonScene));

        ShakeSecurityCam(1.0f);
    }

    private IEnumerator StartSceneSwitch(GameObject scene1, LightingPresets pres2, GameObject scene2)
    {
        Debug.Log("Started Scene Switch. ");

        float passedTime = 0f;
        bool onOrOff = false;

        _lightingControl.ChangeLighting(pres2);

        while (passedTime < 0.13f)
        {
            scene1.SetActive(onOrOff);
            scene2.SetActive(!onOrOff);

            onOrOff = !onOrOff;

            passedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.16f - passedTime);
        }

        scene1.SetActive(false);
        scene2.SetActive(true);
    }

    public void EndTimeline()
    {
        _timeline.Stop();
        _timeline.playableAsset = null;
    }

    public void EndCutscene()
    {
        _cameraSwitcher.SwitchToPlayerCamera();
        _playerBehaviorControl.EnableDisablePlayer(true);
        _pissyMissy.SetTrigger("Idle");
    }

    public void SummonDemon()
    {
        ShakeSecurityCam(2.0f);
        _demonObject.SetActive(true);
    }
    public void ScarePlayer()
    {
        _pissyMissy.SetTrigger("Scare");
    }

    public void UnSummonDemon()
    {
        Debug.Log("unsommoning circle");
        _animator.SetTrigger("Circle");

        StartCoroutine(
                StartSceneSwitch(_demonScene, _demonPreset, _postScene));
    }

    private void ShakeSecurityCam(float time)
    {
        Shaker shaker = _cameraSwitcher.GetCurrenIndexCam()?.GetComponent<Shaker>();

        if (shaker != null)
            shaker.Shake(time, 15f);
    }

    public void StartEndCutscene()
    {
        _playerBehaviorControl.EnableDisablePlayer(false);

        _timeline.playableAsset = _endTimeline;
        _timeline.Play();
    }

    public void SwitchEndScene()
    {
        _lightingControl.ChangeLighting(_endPreset);

        _endScene.SetActive(true);
        _baseScene.SetActive(false);
        _demonScene.SetActive(false);
        _postScene.SetActive(false);

        _playerBehaviorControl.ChangePlayerPosition(_endPlayerPosition.position);
        _pissyMissy.SetTrigger("Die");
    }

    public void DisableUIs()
    {
        foreach(GameObject go in _disablePlayerView)
        {
            go.SetActive(false);
        }
    }
    public void DisablePlayer()
    {
        _cameraSwitcher.TurnOffForEnd();
    }

    public void SwitchCam1()
    {
        _cameraSwitcher.SwitchSecurityCamera(0, false);
    }

    public void SwitchCam2()
    {
        _cameraSwitcher.SwitchSecurityCamera(1, false);
    }

    public void SwitchCam3()
    {
        _cameraSwitcher.SwitchSecurityCamera(2, false);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause(bool pauseOrNot)
    {
        if (pauseOrNot)
            _timeline.Pause();
        else _timeline.Play();
    }
}
