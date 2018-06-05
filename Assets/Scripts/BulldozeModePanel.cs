using UnityEngine;

public class BulldozeModePanel : UiPanel<BulldozeModePanel>
{
    [SerializeField] private int _bulldozeRadius = 2;

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

        var bulldozePos = BitMath.RoundToInt(InputModel.Instance.WorldCursorPos);
        BulldozeAroundPos(bulldozePos);
    }

    private void BulldozeAroundPos(Vector2Int bulldozePos)
    {
        for (var i = -_bulldozeRadius + 1; i < _bulldozeRadius; i++)
        {
            for (var j = -_bulldozeRadius + 1; j < _bulldozeRadius; j++)
            {
                var delta = new Vector2Int(i, j);
                if (SpawnController.Instance.DestroyBuilding(bulldozePos + delta))
                {
                    return;
                }
            }
        }
    }

    public void AcceptChanges()
    {
        HidePanel();
    }

    public override void ShowPanel()
    {
        BulldozeModeHint.Instance.ShowPanel();
        BulldozeModeButtons.Instance.ShowPanel();
        MainHUD.Instance.HidePanel();
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        BulldozeModeButtons.Instance.HidePanel();
        BulldozeModeHint.Instance.HidePanel();
        MainHUD.Instance.ShowPanel();
        base.HidePanel();
    }
}