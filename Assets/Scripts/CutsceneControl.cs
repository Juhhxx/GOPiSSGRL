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
    [SerializeField] private GameObject _endScene;
    [SerializeField] private LightingPresets _endPreset;
    [SerializeField] private Transform _newPlayerPosition;
    [SerializeField] private Transform _newPlayerDirection;

    [SerializeField] private Transform _endPlayerPosition;
    [SerializeField] private GameObject _demonObject;
    [SerializeField] private PlayableDirector _demonTimeline;
    [SerializeField] private PlayableDirector _endTimeline;
    [SerializeField] private Animator _pissyMissy;
    private PlayerBehaviorControl _playerBehaviorControl;


    private void Start()
    {
        _playerBehaviorControl = FindFirstObjectByType<PlayerBehaviorControl>();
        _demonObject.SetActive(false);
        _demonScene.SetActive(false);

        _lightingControl.ChangeLighting(_basePreset);
    }

    public void AwakeDemon()
    {
        _playerBehaviorControl.EnableDisablePlayer(false);
        
        _playerBehaviorControl.ChangePlayerPosition(_newPlayerPosition.position);
        _playerBehaviorControl.PlayerLookAt(_newPlayerDirection.position);
        _cameraSwitcher.SwitchSecurityCamera(0, false);

        _demonTimeline.Play();

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
        _demonTimeline.Stop();
        _endTimeline.Stop();
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

    public void PlayerDie()
    {
        _pissyMissy.SetTrigger("Die");
    }

    public void UnSummonDemon()
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

    public void StartEndCutscene()
    {
        _playerBehaviorControl.EnableDisablePlayer(false);
        _playerBehaviorControl.ChangePlayerPosition(_endPlayerPosition.position);

        _lightingControl.ChangeLighting(_endPreset);

        _demonScene.SetActive(false);
        _endScene.SetActive(true);

        _endTimeline.Play();
    }
}
