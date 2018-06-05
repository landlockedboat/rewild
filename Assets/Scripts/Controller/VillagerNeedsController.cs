using System;
using UnityEngine;

public class VillagerNeedsController : BitController
{
    private VillagerModel _villagerModel;
    private VillagerController _villagerController;

    private void Awake()
    {
        _villagerModel = GetComponent<VillagerModel>();
        _villagerController = GetComponent<VillagerController>();
    }

    private void CheckHunger()
    {
        if (!_villagerModel.Hunger.IsNeeded()) return;

        var canEat = true;

        switch (LevelConfiguration.Instance.CurrentDiet)
        {
            case Diet.Simple:
                if (TownController.Instance.GetStored(DropItemType.Bread) <= 0)
                {
                    canEat = false;
                }

                break;
            case Diet.MeatEater:
                if (TownController.Instance.GetStored(DropItemType.Bread) <= 0)
                {
                    canEat = false;
                }

                if (TownController.Instance.GetStored(DropItemType.Meat) <= 0)
                {
                    canEat = false;
                }

                break;
            case Diet.Vegan:
                if (TownController.Instance.GetStored(DropItemType.Bread) <= 0)
                {
                    canEat = false;
                }

                if (TownController.Instance.GetStored(DropItemType.Meat) <= 0)
                {
                    canEat = false;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (!canEat)
        {
//            Debug.Log("Order Eat cancelled: Not enough food for " + LevelConfiguration.Instance.CurrentDiet + " diet.");
            return;
        }

        _villagerController.AddOrder(OrderType.Eat);
    }

    private void CheckSleep()
    {
        if (!_villagerModel.Sleepiness.IsNeeded()) return;

        if (_villagerModel.House == null)
        {
            _villagerModel.House = TownController.Instance.GetAvailableHouse();
            if (_villagerModel.House != null)
            {
                _villagerModel.House.SetOwner(_villagerController);
            }
            else
            {
//                Debug.Log("No available housing for villager" + GetHashCode());
                return;
            }
        }

        _villagerController.AddOrder(OrderType.Sleep);
    }

    public void CheckNeeds()
    {
        CheckHunger();
        CheckSleep();
    }

    public void IncreaseNeeds()
    {
        _villagerModel.Sleepiness.Increase();
        _villagerModel.Hunger.Increase();
        if (_villagerModel.Sleepiness.IsCritical() || _villagerModel.Hunger.IsCritical())
        {
            if (_villagerModel.Sleepiness.HasJustTurnedCritical())
            {
                Logger.Instance.LogWarning("villager_sleepy_log");
            }

            if (_villagerModel.Hunger.HasJustTurnedCritical())
            {
                Logger.Instance.LogWarning("villager_hungry_log");
            }

            IncreaseFear();
        }
        else
        {
            _villagerModel.Fear.Restore(_villagerModel.FearRestoreAmmount);
        }
    }

    private void IncreaseFear()
    {
        if (_villagerModel.CurrentOrderType == OrderType.Leave)
        {
            return;
        }

        _villagerModel.Fear.Increase();

        if (_villagerModel.Fear.HasJustTurnedNeeded())
        {
            Logger.Instance.LogWarning("villager_angry_log");
        }

        if (!_villagerModel.Fear.IsCritical()) return;

        if (_villagerModel.Fear.HasJustTurnedCritical())
        {
            Logger.Instance.LogEmergency("villager_leaving_log");
        }

        _villagerController.AddOrder(OrderType.Leave);
        // CurrentState = State.InitOrder;
        // Debug.Log("Villager " + GetHashCode() + " is leaving...");
    }
}