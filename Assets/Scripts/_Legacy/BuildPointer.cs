using UnityEngine;
using UnityEngine.UI;

public class BuildPointer : MonoBehaviour
{
    [SerializeField] private Image _buildPointerImage;
    [SerializeField] private Text _itemAmmountText;

    private InputModel _inputModel;
    private BuildModel _buildModel;
    
    private void Awake()
    {
        _inputModel = InputModel.Instance;
        _buildModel = BuildModel.Instance;
        InputController.Instance.RegisterCallback(NotificationType.OnCursorClick, OnCursorClick);
        BuildController.Instance.RegisterCallback(NotificationType.OnBuildingBuilt, OnBuildingBuilt);
    }

    private void OnBuildingBuilt()
    {
        UpdateView();
    }

    private void OnEnable()
    {
        transform.position = _LegacyMainCameraController.WorldCenterOfScreen();
        UpdateView();
    }

    private void LateUpdate()
    {
        if (_inputModel.CurrentInputState == InputState.Dragging)
        {
            OnCursorClick();
        }
    }

    private void OnCursorClick()
    {
        var roundedPos = BitMath.RoundToInt(_inputModel.WorldCursorPos);
        transform.position = (Vector2)roundedPos;
        _buildModel.CurrentBuilding.Position = roundedPos;
    }

    private void UpdateView()
    {
        if (_buildModel.CurrentBuilding == null)
        {
            return;
        }

        _buildPointerImage.sprite = _buildModel.CurrentBuilding.DisplaySprite;
        _itemAmmountText.text = _buildModel.CurrentBuilding.InventoryAmmount.ToString();

        if (_buildModel.CurrentBuilding.InventoryAmmount <= 0)
        {
            _buildPointerImage.color = Color.red - new Color(0, 0, 0, .3f);
        }
        else
        {
            _buildPointerImage.color = Color.white - new Color(0, 0, 0, .3f);
        }
    }
}