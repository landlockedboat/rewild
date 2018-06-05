using System;
using UnityEngine;

public class BuildModePanel : UiPanel<BuildModePanel>
{
    public Action<Vector2Int> SelectedBuildCallback;
    public Vector2Int SelectedBuildingDimensions;
    [SerializeField] private float _secondsToReShowBuildPanel = 2f;

    [SerializeField] private GameObject _buildingBuiltFxPrefab;
//    public Sprite BuildingSprite;

    private void Awake()
    {
        InputController.Instance.RegisterCallback(NotificationType.OnCursorClick, OnCursorClick);
    }

    private void OnCursorClick()
    {
        if (!IsPanelVisible)
        {
            return;
        }

        BuildModeHint.Instance.HidePanel();
        BuildModeButtons.Instance.BuildingDimensions = SelectedBuildingDimensions;
        var buildingPos = BitMath.RoundToInt(InputModel.Instance.WorldCursorPos);
        BuildModeButtons.Instance.BuildingPos = buildingPos;
        if (!BuildModeButtons.Instance.IsPanelVisible)
        {
            BuildModeButtons.Instance.ShowPanel();
        }
        else
        {
            BuildModePointer.Instance.MovePointer(buildingPos);
        }
    }

    public void AcceptBuilding()
    {
        SelectedBuildCallback(BuildModePointer.Instance.BuildingPos);
        BuildModeButtons.Instance.HidePanel();
        Instantiate(_buildingBuiltFxPrefab, BuildModePointer.Instance.BuildingCenter, Quaternion.identity);
        Invoke(nameof(HidePanel), _secondsToReShowBuildPanel);
    }

    public void CancelBuilding()
    {
        HidePanel();
    }

    public override void ShowPanel()
    {
        BuildModeHint.Instance.ShowPanel();
        BuildPanel.Instance.HidePanel();
        MainHUD.Instance.HidePanel();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        BuildModeHint.Instance.HidePanel();
        BuildModeButtons.Instance.HidePanel();
        BuildPanel.Instance.ShowPanel();
        base.HidePanel();
    }
}