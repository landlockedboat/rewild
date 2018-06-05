using UnityEngine;
using UnityEngine.EventSystems;

public class EditorInputView : BitView, IReadableInput
{
    public Vector2 GetScreenCursorPos()
    { 
        return Input.mousePosition;
    }

    public bool GetIsCursorOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool GetIsCursorPressed()
    {
        return Input.GetMouseButton(0);
    }

    public Vector2 GetWorldCursorPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}