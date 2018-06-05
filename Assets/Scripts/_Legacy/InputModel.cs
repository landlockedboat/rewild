using UnityEngine;

public class InputModel : Singleton<InputModel>
{
    [SerializeField] public float ClickTime = .1f;
    [HideInInspector] public Vector2 WorldCursorPos;
    [HideInInspector] public Vector2 ScreenCursorPos;
    [HideInInspector] public bool IsCursorPressed;
    [HideInInspector] public InputState CurrentInputState;
    [HideInInspector] public float CurrentClickTime;
    [HideInInspector] public bool PreviousIsPressed;
    [HideInInspector] public bool PreviousClickUnderUi;
}