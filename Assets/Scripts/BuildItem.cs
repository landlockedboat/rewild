using UnityEngine;
using UnityEngine.UI;

public class BuildItem : MonoBehaviour
{
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _costText;
    [SerializeField] private Button _button;

    private int _cost;

    public void Init(BuildingType type, Vector2 position)
    {
        _cost = LevelConfiguration.Instance.GetCost(type);

        var typeName = type.ToString().ToLower();

        _nameText.text = typeName;
        _costText.text = _cost.ToString();

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;

        _button.onClick.AddListener(() => { SetBuildModePanelCallback(type); });
        _button.onClick.AddListener(BuildPanel.Instance.HidePanel);
        _button.onClick.AddListener(BuildModePanel.Instance.ShowPanel);

        CheckIfCanAfford();
    }

    private void OnEnable()
    {
        CheckIfCanAfford();
    }

    private void CheckIfCanAfford()
    {
        if (_button)
        {
            _button.interactable = _cost <= LevelConfiguration.Instance.Money;
        }
    }

    private void SetBuildModePanelCallback(BuildingType buildingType)
    {
        var buildingDimensions = Vector2Int.zero;
        switch (buildingType)
        {
            case BuildingType.House:
                buildingDimensions = new Vector2Int(2, 1);
                break;
            case BuildingType.Warehouse:
                buildingDimensions = Vector2Int.one * 2;
                break;
            case BuildingType.Oven:
                buildingDimensions = Vector2Int.one;
                break;
            case BuildingType.WheatFarm:
                buildingDimensions = Vector2Int.one;
                break;
            case BuildingType.Slaughterhouse:
                buildingDimensions = new Vector2Int(2, 1);
                break;
            case BuildingType.Pen:
                buildingDimensions = Vector2Int.one * 2;
                break;
            case BuildingType.TofuFermenter:
                buildingDimensions = Vector2Int.one;
                break;
            case BuildingType.TofuFarm:
                buildingDimensions = Vector2Int.one;
                break;
            default:
                Debug.LogError($"Not recognized buildingType {buildingType}");
                break;
        }

        BuildModePanel.Instance.SelectedBuildCallback = pos =>
        {
            SpawnController.Instance.SpawnBuilding(buildingType, pos);
            LevelConfiguration.Instance.Money -= _cost;
        };

        BuildModePanel.Instance.SelectedBuildingDimensions = buildingDimensions;
    }
}