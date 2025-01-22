using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputModule : StandaloneInputModule
{
    public float renderTextureWidth = 320f;
    public float renderTextureHeight = 180f;
    public EventSystem eventSystem;

    // Override the Process method to adjust the mouse position
    public override void Process()
    {
        // Get the current mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;

        // Normalize and map the mouse position to the render texture space
        float normalizedMouseX = mousePosition.x / Screen.width;
        float normalizedMouseY = mousePosition.y / Screen.height;
        float mappedMouseX = normalizedMouseX * renderTextureWidth;
        float mappedMouseY = normalizedMouseY * renderTextureHeight;

        // Create a new PointerEventData with the corrected position
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = new Vector2(mappedMouseX, mappedMouseY),
            button = PointerEventData.InputButton.Left // Optionally set the button state
        };

        // Handle the mouse press
        HandlePointerPress(pointerData);

        // Handle mouse movement and dragging (if any)
        HandlePointerMove(pointerData);
        HandlePointerDrag(pointerData);

        // Optionally call the base.Process() to process other input events normally
        base.Process();
    }

    private void HandlePointerPress(PointerEventData pointerData)
    {
        // Handle mouse button down
        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            // Check for press
            ExecuteEvents.Execute(pointerData.pointerPress, pointerData, ExecuteEvents.pointerDownHandler);
        }

        // Handle mouse button up
        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            // Check for release
            ExecuteEvents.Execute(pointerData.pointerPress, pointerData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(pointerData.pointerPress, pointerData, ExecuteEvents.pointerClickHandler);
        }
    }

    private void HandlePointerMove(PointerEventData pointerData)
    {
        // Handle pointer move
        ExecuteEvents.Execute(pointerData.pointerEnter, pointerData, ExecuteEvents.pointerMoveHandler);
    }

    private void HandlePointerDrag(PointerEventData pointerData)
    {
        // Handle pointer drag
        if (pointerData.dragging)
        {
            ExecuteEvents.Execute(pointerData.pointerDrag, pointerData, ExecuteEvents.dragHandler);
        }
    }
}
