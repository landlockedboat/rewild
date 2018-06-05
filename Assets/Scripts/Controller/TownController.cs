using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TownController : BitController<TownController>
{
    private TownModel _townModel;
    private SpawnController _spawnController;
    private CellMapController _cellMapController;
    private float _villagerSpawnEachDayLikelyhood;

    public bool DebugUseLoadedGame = true;

    public bool ExistsOrderOfType(OrderType orderType)
    {
        return _townModel.TownOrders.Any(order => order.OrderType == orderType);
    }

    public int GetStored(DropItemType type)
    {
        return _townModel.Storage[type];
    }

    private IEnumerable<FactoryController> GetFactoryControllers(BuildingType buildingType)
    {
        return _townModel.Buildings[buildingType].Select((controller) => (FactoryController) controller).ToList();
    }

    public bool AreAllFactoriesFull(BuildingType buildingType)
    {
        var factories = GetFactoryControllers(buildingType);
        return factories.Count((controller) => !controller.IsFull) <= 0;
    }


    public void AddToStorage(DropItemType type, uint ammount)
    {
        _townModel.Storage[type] += (int) ammount;
        if (ammount == 0)
        {
            return;
        }

        TriggerCallback(NotificationType.OnWarehouseStorageChanged);
    }

    public void RemoveFromStorage(DropItemType type, uint ammount)
    {
        _townModel.Storage[type] -= (int) ammount;
        TriggerCallback(NotificationType.OnWarehouseStorageChanged);
    }

    public void AddToStorage(Dictionary<DropItemType, int> inventory)
    {
        foreach (var pair in inventory)
        {
            AddToStorage(pair.Key, (uint) pair.Value);
        }
    }


    public int VillagersInIsland
    {
        get { return _townModel.VillagersInIsland; }
        set
        {
            if (_townModel.VillagersInIsland > value)
            {
                ++_townModel.VillagersLeftTheIsland;
                if (_townModel.VillagersLeftTheIsland >= _townModel.MaxVillagersLeaving)
                {
                    TriggerCallback(NotificationType.OnGameOver);
                }
            }

            _townModel.VillagersInIsland = value;

            TriggerCallback(NotificationType.OnVillagerAmmountChanged);
        }
    }

    public void PushNewOrder(OrderType orderType, Vector2Int pos)
    {
        var cell = CellMapController.Instance.GetCell(pos);
        PushNewOrder(orderType, cell);
    }

    public void PushNewOrder(OrderType orderType, Cell cell = null, uint ammount = 1)
    {
        _townModel.TownOrders.Enqueue(new TownOrder {OrderType = orderType, Cell = cell, Ammount = ammount});
    }

    public void PushNewOrder(TownOrder townOrder)
    {
        _townModel.TownOrders.Enqueue(townOrder);
    }

    public TownOrder GetNextOrder()
    {
        return _townModel.TownOrders.Count > 0 ? _townModel.TownOrders.Dequeue() : null;
    }

    private void Awake()
    {
        _townModel = TownModel.Instance;
        _spawnController = SpawnController.Instance;

        _townModel.DroppedItems = new List<DroppedItemController>();
        _townModel.TownOrders = new Queue<TownOrder>();
        _townModel.WheatPlants = new List<CropController>();
    }

    private void Start()
    {
        _villagerSpawnEachDayLikelyhood = LevelConfiguration.Instance.VillagerSpawnChance;
        _cellMapController = CellMapController.Instance;
        TimeController.Instance.RegisterCallback(NotificationType.OnDayPassed, OnDayPassed);
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        // Wait 1 frame
        yield return 0;
        var res = PlayerPrefs.GetInt(PlayerPrefsKeys.HasActiveGame.ToString());
        if (res == 1 && DebugUseLoadedGame)
        {
            TownModel.Instance.FromData(
                LoadSaveGame.LoadGameData()
            );
        }
    }

    private void OnDayPassed()
    {
        if (Random.value >= _villagerSpawnEachDayLikelyhood)
        {
            return;
        }

        SpawnController.Instance.SpawnVillager();
        Logger.Instance.LogAdvantage("villager_spawned_log");
    }

    public void HarvestCrop(Vector2Int pos)
    {
        var cell = CellMapController.Instance.GetCell(pos);
        var crop = cell.GetCellObject<CropController>();

        if (crop == null)
        {
            Debug.LogError($"No plant to harvest at {pos}");
            return;
        }

        var farm = cell.GetCellObject<FarmController>();

        switch (farm.BuildingType)
        {
            case BuildingType.TofuFarm:
                _townModel.TofuPlants.Remove(crop);
                _spawnController.SpawnDroppedItem(DropItemType.Soybean, pos);
                PushNewOrder(OrderType.PlantTofu, pos);
                break;
            case BuildingType.WheatFarm:
                _townModel.WheatPlants.Remove(crop);
                _spawnController.SpawnDroppedItem(DropItemType.Wheat, pos);
                PushNewOrder(OrderType.PlantWheat, pos);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        cell.CellObjects.Remove(crop);
        Destroy(crop.gameObject);
    }

    public HouseController GetAvailableHouse()
    {
        foreach (var buildingController in _townModel.Buildings[BuildingType.House])
        {
            var house = (HouseController) buildingController;
            if (!house.HasOwner())
            {
                return house;
            }
        }

        return null;
    }

    public Transform GetClosestBuilding(BuildingType buildingType, Vector2Int position)
    {
        return GetClosestCellObjectInCollection(_townModel.Buildings[buildingType], position);
    }

    private Transform GetClosestCellObjectInCollection(
        IEnumerable<CellObjectController> collection, Vector2Int position)
    {
        var minDist = float.MaxValue;
        Transform ret = null;
        foreach (var cellObject in collection)
        {
            var dist = Vector2Int.Distance(position, BitMath.RoundToInt((Vector2) cellObject.transform.position));
            if (dist >= minDist)
            {
                continue;
            }

            ret = cellObject.transform;
            minDist = dist;
        }

        return ret;
    }

    public DropItemType DestroyItem(Vector2Int pos, out int ammount)
    {
        DroppedItemController target = null;
        foreach (var item in _townModel.DroppedItems)
        {
            if (BitMath.RoundToInt((Vector2) item.transform.position) != pos) continue;
            target = item;
            break;
        }

        if (target == null)
        {
            throw new Exception("No item to destroy at " + pos);
        }

        var type = target.Type;
        ammount = target.Ammount;
        _townModel.DroppedItems.Remove(target);
        Destroy(target.gameObject);

        return type;
    }

    public void DropRawItemInFactory(Vector2Int position, int ammount)
    {
        var factory = _cellMapController.GetCell(position).GetCellObject<FactoryController>();

        if (factory == null)
        {
            Debug.LogError("No factory at " + position);
            return;
        }

        factory.AddRawItems(ammount);
    }

    public Transform GetClosestAvailableFactory(BuildingType buildingType, Vector2Int position)
    {
        var collection = GetFactoryControllers(buildingType).Where(controller => !controller.IsFull).ToList();
        return !collection.Any() ? null : GetClosestCellObjectInCollection(collection, position);
    }
}