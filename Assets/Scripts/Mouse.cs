using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    private static float _globalSensitivity = 1;
    [SerializeField] private Slider _slider;

    private void Start()
    {
       _slider.onValueChanged.AddListener(delegate {UpdateSensitivity();});
       UpdateSensitivity();
    }
    public void UpdateSensitivity()
    {
        _globalSensitivity = _slider.value;
    }
    public static float GetAxisX()
    {
        return Input.GetAxis("Mouse X") * _globalSensitivity;
    }
    public static float GetAxisY()
    {
        return Input.GetAxis("Mouse Y") * _globalSensitivity;
    }
}
