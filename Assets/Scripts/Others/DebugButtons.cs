using UnityEngine;
using UnityEngine.EventSystems;

public class DebugButtons : MonoBehaviour
{
    void Update()
    {
        // if (EventSystem.current.IsPointerOverGameObject())
            // Debug.Log($"Over {name} at {Input.mousePosition}");
    }

    /*
    void OnGUI()
    {
        Vector2 mousePosition = Input.mousePosition;
        GUI.Label(new Rect(10, 10, 200, 20), $"Mouse: {mousePosition}");
    }*/
}
