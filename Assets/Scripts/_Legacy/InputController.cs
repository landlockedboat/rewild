using System;
using UnityEngine;

public class InputController : BitController<InputController>
{
    private InputModel _inputModel;
    
    private IReadableInput _readableInputView;

    [SerializeField] private MobileInputView _mobileInputView;
    [SerializeField] private EditorInputView _editorInputView;
    
    private void Awake()
    {
        _inputModel = InputModel.Instance;

#if UNITY_ANDROID
        _readableInputView = _mobileInputView;
#endif

#if UNITY_EDITOR
        _readableInputView = _editorInputView;
#endif
    }
    
    private void Update()
    {
        switch (_inputModel.CurrentInputState)
        {
            case InputState.NoInput:
                NoInputState();
                break;
            case InputState.CheckingClick:
                CheckingClickState();
                break;
            case InputState.Dragging:
                DraggingState();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _inputModel.WorldCursorPos = _readableInputView.GetWorldCursorPos();
        _inputModel.ScreenCursorPos = _readableInputView.GetScreenCursorPos();
    }

    private void DraggingState()
    {
        _inputModel.IsCursorPressed = GetIsValidPress();
        if (_inputModel.IsCursorPressed)
        {
            return;
        }

        _inputModel.CurrentInputState = InputState.NoInput;
        TriggerCallback(NotificationType.DragEnd);
    }

    private void CheckingClickState()
    {
        _inputModel.IsCursorPressed = GetIsValidPress();
        if (_inputModel.IsCursorPressed)
        {
            _inputModel.CurrentClickTime += Time.deltaTime;
            if (_inputModel.CurrentClickTime >= _inputModel.ClickTime)
            {
                _inputModel.CurrentInputState = InputState.Dragging;
                TriggerCallback(NotificationType.DragBegin);
            }

            return;
        }

        _inputModel.CurrentInputState = InputState.NoInput;
        TriggerCallback(NotificationType.OnCursorClick);
    }

    private void NoInputState()
    {
        _inputModel.IsCursorPressed = GetIsValidPress();
        if (!_inputModel.IsCursorPressed)
        {
            return;
        }

        _inputModel.CurrentClickTime = 0f;
        _inputModel.CurrentInputState = InputState.CheckingClick;
    }
    
    private bool GetIsValidPress()
    {
        if (!_readableInputView.GetIsCursorPressed())
        {
            _inputModel.PreviousIsPressed = false;
            _inputModel.PreviousClickUnderUi = false;
            return false;
        }

        if (_readableInputView.GetIsCursorOverUi())
        {
            if (_inputModel.PreviousIsPressed)
            {
                _inputModel.PreviousClickUnderUi = false;
                return true;
            }

            _inputModel.PreviousClickUnderUi = true;
            return false;
        }

        _inputModel.PreviousIsPressed = !_inputModel.PreviousClickUnderUi;
        return _inputModel.PreviousIsPressed;
    }
    
}