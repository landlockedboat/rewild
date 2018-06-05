using System;
using UnityEngine;

public class PlayerController : HumanoidController
{
    // Manual Singleton
    private static PlayerController _instance;
    public static PlayerController Instance => SingletonHelper.GetInstance(ref _instance);
    
    private InputModel _inputModel;
    private bool _hasToMove;
    private NotificationType _lastNotificationType;
    private Vector2 _lastClickPos;
    private PlayerModel _playerModel;

    private State _currentState = State.AcceptsMovement;

    public enum State
    {
        AcceptsMovement,
        OnUi
    }

    protected override void Awake()
    {
        base.Awake();
        _inputModel = InputModel.Instance;
        _playerModel = (PlayerModel) HumanoidModel;
        InputController.Instance.RegisterCallback(UpdateLastNotificationType);
        _lastClickPos = (Vector3)BitMath.RoundToInt(transform.position);
        _hasToMove = true;
    }

    private void UpdateLastNotificationType(NotificationType obj)
    {
        switch (_currentState)
        {
            case State.AcceptsMovement:
                _lastNotificationType = obj;
                break;
            case State.OnUi:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeState(State nextState)
    {
        switch (nextState)
        {
            case State.AcceptsMovement:
                break;
            case State.OnUi:
                _lastNotificationType = NotificationType.None;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
        
        _currentState = nextState;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.AcceptsMovement:
                AcceptsMovementState();
                break;
            case State.OnUi:
                if (_hasToMove)
                {
                    _hasToMove = Move(_lastClickPos);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AcceptsMovementState()
    {
        switch (_lastNotificationType)
        {
            case NotificationType.None:
                if (_hasToMove)
                {
                    _hasToMove = Move(_lastClickPos);
                }
                break;
            case NotificationType.OnCursorClick:
                _lastClickPos = BitMath.RoundToInt(_inputModel.WorldCursorPos);
                _lastNotificationType = NotificationType.None;
                _hasToMove = true;
                break;
            case NotificationType.DragBegin:
                Move(_inputModel.WorldCursorPos);
                break;
            case NotificationType.DragEnd:
                _lastClickPos = BitMath.RoundToInt(_inputModel.WorldCursorPos);
                _lastNotificationType = NotificationType.None;
                _hasToMove = true;
                break;
        }
    }

    

    public void AddMoney(int ammount)
    {
        _playerModel.Money += ammount;
        TriggerCallback(NotificationType.MoneyUpdated);
    }
}