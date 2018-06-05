using UnityEngine;

public class BuildModeButtons : UiPanel<BuildModeButtons>
{
    [HideInInspector] public Vector2Int BuildingDimensions;
    [HideInInspector] public Vector2Int BuildingPos;

    [SerializeField] private BuildModePointer _buildModePointer;

    public override void ShowPanel()
    {
        _buildModePointer.BuildingDimensions = BuildingDimensions;
        var rt = _buildModePointer.GetComponent<RectTransform>();
        rt.anchoredPosition = BuildingPos;
        _buildModePointer.ShowPanel();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        _buildModePointer.HidePanel();
        base.HidePanel();
    }
}
