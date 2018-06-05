using UnityEngine;

public class PlantUiFloatingText : FloatingText
{
    [SerializeField] private PlantUiView _plantUiView;
    private PlantController _plantController;

    private void Start()
    {
        _plantController = _plantUiView.PlantController;
        _plantController.RegisterCallback(NotificationType.PlantGrown, OnPlantGrown);
    }

    private void OnPlantGrown()
    {
        var rewardString = _plantController.Plant.GrowReward.ToString();
        DisplayText(rewardString, Color.yellow);
    }
}