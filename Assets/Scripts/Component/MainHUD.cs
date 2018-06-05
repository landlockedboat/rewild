using System;
using UnityEngine;
using UnityEngine.UI;

public class MainHUD : UiPanel<MainHUD>
{
    [SerializeField] private Text _wheatText;
    [SerializeField] private Text _breadText;

    [SerializeField] private Text _pigText;
    [SerializeField] private Text _meatText;

    [SerializeField] private Image _pigImage;
    [SerializeField] private Image _meatImage;

    [SerializeField] private Sprite _pigSprite;
    [SerializeField] private Sprite _meatSprite;

    [SerializeField] private Sprite _soySprite;
    [SerializeField] private Sprite _tofuSprite;

    [SerializeField] private Text _villagerText;
    [SerializeField] private Text _villagersLeftText;

    private void Start()
    {
        OnWarehouseStorageChanged();

        TownController.Instance.RegisterCallback(
            NotificationType.OnWarehouseStorageChanged, OnWarehouseStorageChanged);
        TownController.Instance.RegisterCallback(NotificationType.OnVillagerAmmountChanged, OnVillagerAmmountChanged);
    }

    private void OnVillagerAmmountChanged()
    {
        var prev = int.Parse(_villagerText.text);
        var curr = TownController.Instance.VillagersInIsland;
        
        _villagerText.text = curr.ToString();
        
        if (prev <= curr) return;
        
        var prevLeft = int.Parse(_villagersLeftText.text);
        prevLeft += prev - curr;
        _villagersLeftText.text = prevLeft.ToString();
    }

    private void OnWarehouseStorageChanged()
    {
        _wheatText.text = TownController.Instance.GetStored(DropItemType.Wheat).ToString();
        _breadText.text = TownController.Instance.GetStored(DropItemType.Bread).ToString();

        switch (LevelConfiguration.Instance.CurrentDiet)
        {
            case Diet.Simple:
                _pigText.text = "";
                _meatText.text = "";
                _pigImage.color = Color.clear;
                _meatImage.color = Color.clear;
                break;
            case Diet.MeatEater:
                _pigText.text = TownController.Instance.GetStored(DropItemType.Piggy).ToString();
                _meatText.text = TownController.Instance.GetStored(DropItemType.Meat).ToString();
                _pigImage.color = Color.white;
                _meatImage.color = Color.white;
                _pigImage.sprite = _pigSprite;
                _meatImage.sprite = _meatSprite;
                break;
            case Diet.Vegan:
                _pigText.text = TownController.Instance.GetStored(DropItemType.Soybean).ToString();
                _meatText.text = TownController.Instance.GetStored(DropItemType.Tofu).ToString();
                _pigImage.color = Color.white;
                _meatImage.color = Color.white;
                _pigImage.sprite = _soySprite;
                _meatImage.sprite = _tofuSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}