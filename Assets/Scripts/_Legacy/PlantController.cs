using System;
using UnityEngine;

public class PlantController : BuildingController
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _plantUiPrefab;

    [HideInInspector] public Plant Plant;
    [HideInInspector] public TimeSpan CurrentTimeSpanToGrow;
    [HideInInspector] public TimeSpan TimeSpanToGrow;
    [HideInInspector] public bool Grown;

    private void Start()
    {
        // Plant = (Plant) Building;
        TimeSpanToGrow = TimeSpan.FromMinutes(Plant.MinutesToGrow);

        LegacyTimeController.Instance.RegisterCallback(NotificationType.OneSecondElapsed, OnOneSecondElapsed);
        _spriteRenderer.sprite = Plant.DisplaySprite;

        var trans = WorldSpaceCanvasController.Instance.transform;
        var go = Instantiate(_plantUiPrefab, transform.position, Quaternion.identity, trans);

        go.GetComponent<PlantUiView>().PlantController = this;
    }

    private void OnOneSecondElapsed()
    {
        if (Grown)
        {
            return;
        }

        CurrentTimeSpanToGrow += TimeSpan.FromSeconds(1);

        if (CurrentTimeSpanToGrow > TimeSpanToGrow)
        {
            Grown = true;
            _spriteRenderer.sprite = Plant.GrowSprite;
            PlayerController.Instance.AddMoney(Plant.GrowReward);
            TriggerCallback(NotificationType.PlantGrown);
        }

        TriggerCallback(NotificationType.OneSecondElapsed);
    }
}