using UnityEngine;
using UnityEngine.UI;

public class CanvasController : BitController<CanvasController>
{
    [SerializeField] private GameObject _buttonsPanel;
    
    [Header("Panels")] 
    [SerializeField] private GameObject _shopUi;
    [SerializeField] private GameObject _optionsUi;
    [SerializeField] private GameObject _buildUi;
    [SerializeField] private GameObject _inventoryUi;

    [Header("Overlays")] 
    [SerializeField] private GameObject _timeView;

    private CanvasScaler _canvasScaler;
    private Vector2 _res;

    public void SetShopUiActive(bool active)
    {
        SetPanelActive(_shopUi, active);
    }

    public void SetOptionsUiActive(bool active)
    {
        SetPanelActive(_optionsUi, active);
    }

    public void SetInventoryUiActive(bool active)
    {
        SetPanelActive(_inventoryUi, active);
    }
    
    public void ShowBuildUi(Building building)
    {
        BuildModel.Instance.CurrentBuilding = building;
        SetPanelActive(_buildUi, true);
    }
    
    public void HideBuildUi()
    {
        SetPanelActive(_buildUi, false);
    }

    public void SetTimeViewActive(bool active)
    {
        SetViewActive(_timeView, active);
    }

    private void SetPanelActive(GameObject go, bool active)
    {
        go.SetActive(active);
        _buttonsPanel.SetActive(!active);
        
        PlayerController.Instance.ChangeState(active
            ? PlayerController.State.OnUi
            : PlayerController.State.AcceptsMovement);
        
    }

    private void SetViewActive(GameObject go, bool active)
    {
        go.SetActive(active);
    }
}