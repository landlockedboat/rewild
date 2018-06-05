using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputView : BitView, IReadableInput
{
    public bool GetIsCursorPressed()
    {
        if (Input.touchCount == 0)
        {
            return false;
        }

        var touch = Input.GetTouch(0);
        return touch.phase != TouchPhase.Canceled || touch.phase != TouchPhase.Ended;
    }

    public Vector2 GetScreenCursorPos()
    {
        if (Input.touchCount == 0)
        {
            return Vector2.zero;
        }
        
        return Input.GetTouch(0).position;
    }

    public bool GetIsCursorOverUi()
    {
        if (Input.touchCount == 0)
        {
            return false;
        }

        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return true;
        }

        return false;
    }

    public Vector2 GetWorldCursorPos()
    {
        return Camera.main.ScreenToWorldPoint(GetScreenCursorPos());
    }
}

public interface IReadableInput
{
    Vector2 GetWorldCursorPos();
    Vector2 GetScreenCursorPos();
    bool GetIsCursorOverUi();
    bool GetIsCursorPressed();
}