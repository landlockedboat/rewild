using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelConfiguration : BitController<LevelConfiguration>
{
    public List<BuildingType> AvailableBuildings;

    [Space]
    [SerializeField]
    private int _money;

    public int Money
    {
        get { return _money; }
        set
        {
            _money = value;
            TriggerCallback(NotificationType.OnMoneyAmmountChanged);
        }
    }

    [Header("Payoff")] [SerializeField] private int _daysTillPayoff = 3;
    public int MinMoneyPerVillager = 25;
    public int MaxMoneyPerVillager = 45;

    [Header("Needs")] public float SecondsToHungerFilled = 60;
    public float SecondsToFearFilled = 60;
    public float SecondsToSleepinessFilled = 60;
    public float FearRestoreAmmount = .001f;

    [Header("Villagers")] public float VillagerSpawnChance = .1f;
    public float VillagerSpeed = 2f;
    public Diet CurrentDiet;

    [Header("Time")] public float SecondsInADay = 120;

    [Header("Prices")] [SerializeField]
    private BuildingPrice _housePrice = new BuildingPrice {Type = BuildingType.House};

    [SerializeField] private BuildingPrice _dockPrice = new BuildingPrice {Type = BuildingType.Dock};
    [SerializeField] private BuildingPrice _ovenPrice = new BuildingPrice {Type = BuildingType.Oven};
    [SerializeField] private BuildingPrice _penPrice = new BuildingPrice {Type = BuildingType.Pen};

    [SerializeField]
    private BuildingPrice _slaughterhousePrice = new BuildingPrice {Type = BuildingType.Slaughterhouse};

    [SerializeField] private BuildingPrice _tofuFarmPrice = new BuildingPrice {Type = BuildingType.TofuFarm};
    [SerializeField] private BuildingPrice _tofuFermenterPrice = new BuildingPrice {Type = BuildingType.TofuFermenter};
    [SerializeField] private BuildingPrice _warehousePrice = new BuildingPrice {Type = BuildingType.Warehouse};
    [SerializeField] private BuildingPrice _wheatFarmPrice = new BuildingPrice {Type = BuildingType.WheatFarm};

    private List<BuildingPrice> _buildingPrices;

    private void Start()
    {
        TimeController.Instance.RegisterCallback(NotificationType.OnDayPassed, OnDayPassed);
        _buildingPrices = new List<BuildingPrice>
        {
            _housePrice,
            _dockPrice,
            _ovenPrice,
            _penPrice,
            _slaughterhousePrice,
            _tofuFarmPrice,
            _tofuFermenterPrice,
            _warehousePrice,
            _wheatFarmPrice
        };
    }

    private void OnDayPassed()
    {
        CheckPayoff();
    }

    public void TriggerVictory()
    {
        TriggerCallback(NotificationType.OnVictoryAchieved);
    }

    private void CheckPayoff()
    {
        if (TimeController.Instance.CurrentDay % _daysTillPayoff != 0)
        {
            return;
        }

        var villagerAmmount = TownController.Instance.VillagersInIsland;
        var moneyPerVillager = Mathf.CeilToInt(Mathf.Lerp(MinMoneyPerVillager, MaxMoneyPerVillager, Random.value));
        var payoff = villagerAmmount * moneyPerVillager;
        Money += payoff;
    }

    public int GetCost(BuildingType type)
    {
        foreach (var price in _buildingPrices)
        {
            if (price.Type == type)
            {
                return price.Cost;
            }
        }

        Debug.LogError($"Found no type {type} in the collection.");
        return 0;
    }
}

[Serializable]
public class BuildingPrice
{
    public BuildingType Type;
    public int Cost;
}