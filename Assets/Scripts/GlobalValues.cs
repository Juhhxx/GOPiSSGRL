using UnityEngine;
using UnityEngine.UI;

public class GlobalValues : MonoBehaviour
{
    private static float _globalSensitivity = 1f;
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private float _maxSensitivity = 10f;
    [SerializeField] private float _maxVolume = 3f;

    private void Start()
    {
       _sensitivitySlider.onValueChanged.AddListener(delegate {UpdateSensitivity();});
       UpdateSensitivity();

       _volumeSlider.onValueChanged.AddListener(delegate {UpdateVolume();});
       UpdateVolume();
    }
    public void UpdateSensitivity()
    {
        _globalSensitivity = _maxSensitivity * (_sensitivitySlider.value - _sensitivitySlider.minValue)
            / (_sensitivitySlider.maxValue - _sensitivitySlider.minValue);
        _globalSensitivity = Mathf.Max(0.1f, _globalSensitivity);
    }
    public static float GetAxisX()
    {
        return Input.GetAxis("Mouse X") * _globalSensitivity;
    }
    public static float GetAxisY()
    {
        return Input.GetAxis("Mouse Y") * _globalSensitivity;
    }
    public void UpdateVolume()
    {
        AudioListener.volume = _maxVolume * (_volumeSlider.value - _volumeSlider.minValue)
            / (_volumeSlider.maxValue - _volumeSlider.minValue);
    }
}
