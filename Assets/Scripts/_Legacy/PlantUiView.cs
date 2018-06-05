using System;
using UnityEngine;
using UnityEngine.UI;

public class PlantUiView : BitView
{
    [SerializeField] private Text _plantUiText;
    [HideInInspector] public PlantController PlantController;

    private void Start()
    {
        PlantController.RegisterCallback(NotificationType.OneSecondElapsed, UpdateTime);
        PlantController.RegisterCallback(NotificationType.PlantGrown, OnPlantGrown);
        UpdateTime();
    }

    private void OnPlantGrown()
    {
        _plantUiText.enabled = false;
        PlantController.UnregisterCallback(NotificationType.OneSecondElapsed, UpdateTime);
    }

    private void UpdateTime()
    {
        var timespan = PlantController.TimeSpanToGrow - PlantController.CurrentTimeSpanToGrow;
        var dateTime = new DateTime(timespan.Ticks);
        _plantUiText.text =
            dateTime.ToString("HH:mm:ss");
    }
}