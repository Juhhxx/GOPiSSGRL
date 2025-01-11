using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.TextCore;

public class Radio : MonoBehaviour
{
    [SerializeField] private float _frequency;
    [SerializeField] private float _shakeStrenght = 0.03f;
    [SerializeField] private float _volumeChangeDistance;
    [SerializeField] private TextMeshPro _frequencyDisplay;
    [SerializeField] private AudioClip _demonSound;
    [SerializeField] private AudioClip _defaultSound;
    [SerializeField] private RadioChannels[] _radioChannels;
    private Vector3 _intialPosition;
    private RotateWhenHolding _rotateHolding;
    private AudioSource _audioSource;
    private SummonDemon _summonDemon;

    private void Start()
    {
        _intialPosition = transform.localPosition;
        _rotateHolding = GetComponentInChildren<RotateWhenHolding>();
        _audioSource = GetComponent<AudioSource>();
        _summonDemon = FindAnyObjectByType<SummonDemon>();
    }
    private void Update()
    {
        // You need to first round the frequency to the correct number of decimals because otherwise
        // _frequencyDisplay.text wont match the corrected frequency later (because the text is rounding it, so when the
        // frequency value gets there and is floored, if its ex: 80.95 it will display 81.0 but correctedFrequency it will be floored to 80)
        _frequency = Mathf.Round(_rotateHolding.GetCurrentValue() * 10.0f) * 0.1f;

        CheckFrequency();
        _frequencyDisplay.text =  $"{_frequency} MHz";
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
            ChangeAudioVolumeDistance(5f);
            
            foreach (RadioChannels channel in _radioChannels)
            {
                if (channel.Frequency == correctedFrequency)
                {
                    ChangeAudio(channel.Audio);
                    return;
                }
            }

            ChangeAudio(_defaultSound);
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
        _audioSource.volume = Mathf.InverseLerp(20f,0.5f,distance);
    }
    private void ShakeRadioByDistance(float distance)
    {
        float shakeForce = Mathf.InverseLerp(1.5f,0.5f,distance);
        Vector3 direction = Random.insideUnitSphere;
        direction *= shakeForce * _shakeStrenght;

        Vector3 currentPos = _intialPosition;
        currentPos += direction;
        transform.localPosition = currentPos;
    }
    private void ChangeAudio(AudioClip audio)
    {
        if (audio != _audioSource.clip)
        {
            _audioSource.clip = audio;
            _audioSource.Play();
        }
    }
    
}