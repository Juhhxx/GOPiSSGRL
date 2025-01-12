using UnityEngine;

public class KeypadUI : MonoBehaviour
{
    private KeypadControl _control;
    private void Awake()
    {
        _control = FindFirstObjectByType<KeypadControl>();
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Locking");
    }
}
