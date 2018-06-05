using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private float _dampAmmountPerOrthoSizeUnit = .5f;
    private float _dampAmmount;

    private Vector2 _cameraSize;
    private Camera _camera;
    private CellMapController _cellMapController;

    private Vector2 _oldDragPos;
    private Vector2 _oldCameraPos;

    private bool _isDragging;

    private int _screenWidth;
    private float _orthoSize;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraSize = new Vector2(_camera.orthographicSize * Screen.width / Screen.height, _camera.orthographicSize);
        InputController.Instance.RegisterCallback(NotificationType.DragEnd, OnDragEnd);
        InputController.Instance.RegisterCallback(NotificationType.DragBegin, OnDragBegin);
        _screenWidth = Screen.width;
        SetDampAmmount();
    }

    private void SetDampAmmount()
    {
        _orthoSize = _camera.orthographicSize;
        _dampAmmount = _dampAmmountPerOrthoSizeUnit * _orthoSize;
    }

    private void Start()
    {
        _cellMapController = CellMapController.Instance;
    }

    private void OnDragBegin()
    {
        _isDragging = true;
        _oldDragPos = InputModel.Instance.ScreenCursorPos;
        _oldCameraPos = transform.position;
    }

    private void OnDragEnd()
    {
        _isDragging = false;
    }

    private void Update()
    {
        if (!_isDragging)
        {
            return;
        }

        var newDragPos = InputModel.Instance.ScreenCursorPos;

        var diff = (newDragPos - _oldDragPos) / _screenWidth;

        var newPos = _oldCameraPos - diff * _dampAmmount;

        var right = newPos.x + _cameraSize.x > _cellMapController.UpperBoundary.x - 1;
        var left = newPos.x - _cameraSize.x < _cellMapController.LowerBoundary.x + 1;
        if (left || right)
        {
            newPos.x = transform.position.x;
        }

        var up = newPos.y + _cameraSize.y > _cellMapController.UpperBoundary.y - 1;
        var down = newPos.y - _cameraSize.y < _cellMapController.LowerBoundary.y + 1;

        if (down || up)
        {
            newPos.y = transform.position.y;
        }

        transform.position = new Vector3(newPos.x, newPos.y, -10);
    }
}