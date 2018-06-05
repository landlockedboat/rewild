using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VillagerStateController : BitController
{
    private VillagerModel _villagerModel;
    private VillagerController _villagerController;
    private VillagerView _villagerView;
    private VillagerNeedsController _villagerNeeds;
    private TownController _townController;
    private SpawnController _spawnController;

    private uint OrderAmmount => _villagerModel.CurrentTownOrder.Ammount;

    public VillagerState CurrentVillagerState
    {
        get { return _villagerModel.CurrentVillagerState; }
        set
        {
            ChangeState(_villagerModel.CurrentVillagerState, value);
            _villagerModel.CurrentVillagerState = value;
            TriggerCallback(NotificationType.OnStateChanged);
        }
    }

    private void Awake()
    {
        _villagerModel = GetComponent<VillagerModel>();
        _villagerNeeds = GetComponent<VillagerNeedsController>();
        _villagerView = GetComponent<VillagerView>();
        _villagerController = GetComponent<VillagerController>();
        _townController = TownController.Instance;
        _spawnController = SpawnController.Instance;
    }

    public void UpdateState()
    {
        switch (CurrentVillagerState)
        {
            case VillagerState.Idle:
                IdleState();
                break;
            case VillagerState.GetToOrder:
                GetToOrderState();
                break;
            case VillagerState.ProcessOrder:
                ProcessOrderState();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void IdleState()
    {
        _villagerNeeds.CheckNeeds();

        if (_villagerModel.Orders.Count > 0)
        {
            _villagerController.CurrentOrderType = _villagerController.GetNextOrder();
            return;
        }

        var order = _townController.GetNextOrder();

        if (order != null)
        {
            _villagerModel.CurrentTownOrder = order;
            _villagerController.CurrentOrderType = _villagerModel.CurrentTownOrder.OrderType;
            return;
        }

        if (Random.Range(_villagerModel.MinSecondsIdle, _villagerModel.MaxSecondsIdle) <
            _villagerModel.SecondsIdle)
        {
            _villagerController.CurrentOrderType = OrderType.Roam;
            return;
        }

        _villagerModel.SecondsIdle += Time.deltaTime;
    }

    private void GetToOrderState()
    {
        var firstNode = _villagerModel.CurrentPath.First();
        var destination = firstNode.Position;

        if (!_villagerView.Move(destination)) return;

        _villagerModel.CurrentPath.Remove(firstNode);
        _villagerController.Cell = CellMapController.Instance.GetCell(transform.position);

        if (_villagerModel.CurrentPath.Count <= 0)
        {
            CurrentVillagerState = VillagerState.ProcessOrder;
        }
    }

    private void ProcessOrderState()
    {
        var orderCompleted = true;
        switch (_villagerModel.CurrentOrderType)
        {
            case OrderType.Eat:
                ProcessEatOrder();
                break;
            case OrderType.Sleep:
                _villagerModel.Sleepiness.Restore(_villagerModel.House.SleepinessRestore);
                if (!_villagerModel.Sleepiness.IsRestored())
                {
                    orderCompleted = false;
                }

                break;
            case OrderType.Roam:
                break;
            case OrderType.Leave:
                --_townController.VillagersInIsland;
                Destroy(gameObject);
                break;
            case OrderType.PlantWheat:
                _spawnController.SpawnWheatCrop(_villagerModel.CurrentTownOrder.Position);
                break;
            case OrderType.PlantTofu:
                _spawnController.SpawnTofuCrop(_villagerModel.CurrentTownOrder.Position);
                break;
            case OrderType.Null:
                throw new ArgumentOutOfRangeException();
            case OrderType.PickItem:
                int ammount;
                var type = _townController.DestroyItem(_villagerModel.CurrentTownOrder.Position, out ammount);
                _villagerModel.Inventory[type] += ammount;
                _villagerController.AddOrder(OrderType.DropItem);
                break;
            case OrderType.DropItem:
                _townController.AddToStorage(_villagerModel.Inventory);
                _villagerModel.ResetInventory();
                break;
            case OrderType.HarvestPlant:
                _townController.HarvestCrop(_villagerModel.CurrentTownOrder.Position);
                break;
            case OrderType.BakeBreadFindWheat:
                GrabItemFromTownStorage(DropItemType.Wheat, OrderAmmount);
                _villagerController.AddOrder(OrderType.BakeBreadDropWheatInOven);
                break;
            case OrderType.GrowPigDropWheatInPen:
            case OrderType.BakeBreadDropWheatInOven:
                _townController.DropRawItemInFactory(_villagerModel.DestinationCell.Position,
                    _villagerModel.Inventory[DropItemType.Wheat]);
                _villagerModel.Inventory[DropItemType.Wheat] -= (int) OrderAmmount;
                break;
            case OrderType.FermentTofuFindSoybean:
                GrabItemFromTownStorage(DropItemType.Soybean, OrderAmmount);
                _villagerController.AddOrder(OrderType.FermentTofuDropSoybeanInFermenter);
                break;
            case OrderType.FermentTofuDropSoybeanInFermenter:
                _townController.DropRawItemInFactory(_villagerModel.DestinationCell.Position,
                    _villagerModel.Inventory[DropItemType.Soybean]);
                _villagerModel.Inventory[DropItemType.Soybean] -= (int) OrderAmmount;
                break;
            case OrderType.MakeMeatDropPiggyInSlaughterhouse:
                _townController.DropRawItemInFactory(_villagerModel.DestinationCell.Position,
                    _villagerModel.Inventory[DropItemType.Piggy]);
                _villagerModel.Inventory[DropItemType.Piggy] -= (int) OrderAmmount;
                break;
            case OrderType.FeedPigFindWheat:
                GrabItemFromTownStorage(DropItemType.Wheat, OrderAmmount);
                _villagerController.AddOrder(OrderType.GrowPigDropWheatInPen);
                break;
            case OrderType.MakeMeatFindPig:
                GrabItemFromTownStorage(DropItemType.Piggy, OrderAmmount);
                _villagerController.AddOrder(OrderType.MakeMeatDropPiggyInSlaughterhouse);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (orderCompleted)
        {
            CurrentVillagerState = VillagerState.Idle;
        }
    }

    private void ProcessEatOrder()
    {
        var success = true;
        Action eatCallback;

        switch (LevelConfiguration.Instance.CurrentDiet)
        {
            case Diet.Simple:
                success &= TownStorageContains(DropItemType.Bread, OrderAmmount);
                eatCallback = () =>
                {
                    GrabItemFromTownStorage(DropItemType.Bread, OrderAmmount);
                    _villagerModel.Inventory[DropItemType.Bread] -= (int) OrderAmmount;
                };
                break;
            case Diet.MeatEater:
                success &= TownStorageContains(DropItemType.Bread, OrderAmmount);
                success &= TownStorageContains(DropItemType.Meat, OrderAmmount);
                eatCallback = () =>
                {
                    GrabItemFromTownStorage(DropItemType.Bread, OrderAmmount);
                    _villagerModel.Inventory[DropItemType.Bread] -= (int) OrderAmmount;
                    GrabItemFromTownStorage(DropItemType.Meat, OrderAmmount);
                    _villagerModel.Inventory[DropItemType.Meat] -= (int) OrderAmmount;
                };
                break;
            case Diet.Vegan:
                success &= TownStorageContains(DropItemType.Bread, OrderAmmount);
                success &= TownStorageContains(DropItemType.Tofu, OrderAmmount);
                eatCallback = () =>
                {
                    GrabItemFromTownStorage(DropItemType.Bread, OrderAmmount);
                    _villagerModel.Inventory[DropItemType.Bread] -= (int) OrderAmmount;
                    GrabItemFromTownStorage(DropItemType.Tofu, OrderAmmount);
                    _villagerModel.Inventory[DropItemType.Tofu] -= (int) OrderAmmount;
                };
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (!success)
        {
            Debug.Log($"Order {_villagerModel.CurrentOrderType} could not be completed: Not enough food for " +
                      $"{LevelConfiguration.Instance.CurrentDiet} diet.");
            _villagerController.AddOrder(OrderType.Roam);
            return;
        }

        eatCallback();
        _villagerModel.Hunger.Restore(1f);
    }

    private bool TownStorageContains(DropItemType type, uint ammount)
    {
        return _townController.GetStored(type) >= ammount;
    }

    private bool GrabItemFromTownStorage(DropItemType type, uint ammount)
    {
        if (!TownStorageContains(type, ammount))
        {
            return false;
        }

        _townController.RemoveFromStorage(type, ammount);
        _villagerModel.Inventory[type] += (int) ammount;
        return true;
    }


    public void ChangeState(VillagerState currentVillagerState, VillagerState newVillagerState)
    {
        switch (currentVillagerState)
        {
            case VillagerState.Idle:
                break;
            case VillagerState.GetToOrder:
                break;
            case VillagerState.ProcessOrder:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentVillagerState), currentVillagerState, null);
        }
    }
}