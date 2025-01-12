using System.Collections;
using TMPro;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class Radio : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private float _shakeStrenght = 0.03f;
    [SerializeField] private TextMeshPro _frequencyDisplay;
    [SerializeField] private AudioSource _mainAudio;
    [SerializeField] private AudioSource _staticAudio;
    [SerializeField] private float _fadeAudioSpeed;
    [MinMaxSlider(0.0f, 1.0f)][SerializeField] private Vector2 _staticVolumeChange;
    [SerializeField] private AudioClip _demonSound;
    [SerializeField] private RadioChannels[] _radioChannels;
    private Vector3 _intialPosition;
    private RotateWhenHolding _rotateHolding;
    private SummonDemon _summonDemon;
    private YieldInstruction _wff;

    private void Start()
    {
        _intialPosition = transform.localPosition;
        _rotateHolding = GetComponentInChildren<RotateWhenHolding>();
        _summonDemon = FindAnyObjectByType<SummonDemon>();
        _wff = new WaitForEndOfFrame();
    }
    private void OnEnable()
    {
        StartCoroutine(StaticVolumeChanger());
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        // You need to first round the frequency to the correct number of decimals because otherwise
        // _frequencyDisplay.text wont match the corrected frequency later (because the text is rounding it, so when the
        // frequency value gets there and is floored, if its ex: 80.95 it will display 81.0 but correctedFrequency it will be floored to 80)
        _frequency = Mathf.Round(_rotateHolding.GetCurrentValue() * 10.0f) * 0.1f;

        CheckFrequency();
        _frequencyDisplay.text = $"{_frequency} MHz";
    }
    private void CheckFrequency()
    {
        int pointIndex;
        float correctedFrequency = Mathf.Floor(_frequency);

        if (_summonDemon.ChalkFrequencies.Contains(correctedFrequency))
        {
            pointIndex = _summonDemon.ChalkFrequencies.IndexOf(correctedFrequency);
            Debug.Log(pointIndex);
            DetectDistance(pointIndex);
            ChangeAudio(_demonSound);
        }
        else
        {
            foreach (RadioChannels channel in _radioChannels)
            {
                if (channel.Frequency == correctedFrequency)
                {
                    ChangeAudio(channel.Audio);
                    return;
                }
            }
            TurnOffAudio();
        }
    }
    private void DetectDistance(int index)
    {
        Transform pointTrans = _summonDemon.ChalkPoints[index].transform;

        // Corrects the position so the y value is ignored
        Vector3 correctedPoint = pointTrans.position;
        correctedPoint.y = 0f;

        Vector3 correctedRadio = transform.position; 
        correctedRadio.y = 0f;

        float distance = Vector3.Distance(correctedRadio,correctedPoint);

        ChangeAudioVolumeDistance(distance);
        ShakeRadioByDistance(distance);

        // Debug.Log($"Distance to Radio : {distance}");
    }  
    private void ChangeAudioVolumeDistance (float distance)
    {
        _mainAudio.volume = Mathf.InverseLerp(20f,0.5f,distance);
    }
    private void ShakeRadioByDistance(float distance)
    {
        float shakeForce = Mathf.InverseLerp(1f,0.5f,distance);
        Vector3 direction = Random.insideUnitSphere;
        direction *= shakeForce * _shakeStrenght;

        Vector3 currentPos = _intialPosition;
        currentPos += direction;
        transform.localPosition = currentPos;
    }
    private void ChangeAudio(AudioClip audio)
    {
        if (audio != _mainAudio.clip)
        {
            _mainAudio.clip = audio;
            FadeAudio(true, audio);
        }
    }
    private void TurnOffAudio()
    {
        FadeAudio(false);
    }
    private IEnumerator FadeAudio(bool onOff, AudioClip audio = null)
    {
        if (onOff)
        {
            _mainAudio.clip = audio;
            _staticVolumeChange.y -= _staticVolumeChange.x;
            _staticVolumeChange.x -= _staticVolumeChange.x;
        }
        else
        {
            _mainAudio.clip = null;
            _staticVolumeChange.x += _staticVolumeChange.y;
            _staticVolumeChange.y += _staticVolumeChange.y;
        }

        float startVolume = _mainAudio.volume;
        float   endVolume = onOff ? 0.6f : 0.0f;
        float   newVolume = startVolume;
        float           i = 0;

        while(newVolume >= endVolume)
        {
            newVolume = Mathf.Lerp(startVolume,endVolume,i);

            _mainAudio.volume = newVolume;

            i = _fadeAudioSpeed * Time.deltaTime;

            yield return _wff;
        }
    }
    private IEnumerator StaticVolumeChanger()
    {
        while(true)
        {
            float volume = Random.Range(_staticVolumeChange.x,_staticVolumeChange.y);
            _staticAudio.volume = volume;

            yield return _wff;
        }
    }
}