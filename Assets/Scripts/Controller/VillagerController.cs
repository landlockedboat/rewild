using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VillagerController : CellObjectController
{
    public OrderType CurrentOrderType
    {
        set
        {
            _villagerModel.CurrentOrderType = value;
            InitCurrentOrder();
        }
    }

    private Vector2Int PosInt => BitMath.RoundToInt((Vector2) transform.position);

    private VillagerModel _villagerModel;
    private VillagerNeedsController _villagerNeeds;
    private VillagerStateController _villagerStateController;
    private TownModel _townModel;
    private TownController _townController;
    private CellMapController _cellMapController;

    private void Awake()
    {
        _villagerModel = GetComponent<VillagerModel>();
        _cellMapController = CellMapController.Instance;
        _villagerModel.Rigidbody2D = GetComponent<Rigidbody2D>();
        _villagerModel.Orders = new Queue<OrderType>();
        _townModel = TownModel.Instance;
        _townController = TownController.Instance;
        _villagerNeeds = GetComponent<VillagerNeedsController>();
        _villagerStateController = GetComponent<VillagerStateController>();
    }

    private void Update()
    {
        _villagerStateController.UpdateState();
        _villagerNeeds.IncreaseNeeds();
    }

    private void InitCurrentOrder()
    {
        _villagerModel.SecondsIdle = 0;

        switch (_villagerModel.CurrentOrderType)
        {
            case OrderType.Sleep:
                GoToOrderDestination(_villagerModel.House.Position);
                break;
            case OrderType.Roam:
                GoToOrderDestination(GetRoamDestination());
                break;
            case OrderType.Leave:
                GoToOrderDestination(_townModel.Dock.SpawnCell.Position);
                break;
            case OrderType.PlantWheat:
            case OrderType.PlantTofu:
            case OrderType.PickItem:
            case OrderType.HarvestPlant:
                GoToOrderDestination(_villagerModel.CurrentTownOrder.Position);
                break;
            case OrderType.Null:
                throw new ArgumentOutOfRangeException();
            case OrderType.Eat:
                GoToWarehouse();
                break;
            case OrderType.DropItem:
                GoToWarehouse();
                break;
            case OrderType.FermentTofuFindSoybean:
                GoToWarehouse();
                break;
            case OrderType.BakeBreadFindWheat:
                if (_townController.AreAllFactoriesFull(BuildingType.Oven))
                {
                    _townController.PushNewOrder(_villagerModel.CurrentOrderType);
                    AddOrder(OrderType.Roam);
                    break;
                }

                GoToWarehouse();
                break;
            case OrderType.FeedPigFindWheat:
                if (_townController.AreAllFactoriesFull(BuildingType.Pen))
                {
                    _townController.PushNewOrder(_villagerModel.CurrentOrderType);
                    AddOrder(OrderType.Roam);
                    break;
                }

                GoToWarehouse();
                break;
            case OrderType.MakeMeatFindPig:
                if (_townController.AreAllFactoriesFull(BuildingType.Slaughterhouse))
                {
                    _townController.PushNewOrder(_villagerModel.CurrentOrderType);
                    AddOrder(OrderType.Roam);
                    break;
                }

                GoToWarehouse();
                break;
            case OrderType.BakeBreadDropWheatInOven:
                var oven = _townController.GetClosestAvailableFactory(BuildingType.Oven, PosInt);
                ContinueOrderGoToTransform(oven);
                break;
            case OrderType.FermentTofuDropSoybeanInFermenter:
                var fermenter = _townController.GetClosestAvailableFactory(BuildingType.TofuFermenter, PosInt);
                ContinueOrderGoToTransform(fermenter);
                break;
            case OrderType.MakeMeatDropPiggyInSlaughterhouse:
                var slaughterhouse = _townController.GetClosestAvailableFactory(BuildingType.Slaughterhouse, PosInt);
                ContinueOrderGoToTransform(slaughterhouse);
                break;
            case OrderType.GrowPigDropWheatInPen:
                var pen = _townController.GetClosestAvailableFactory(BuildingType.Pen, PosInt);
                ContinueOrderGoToTransform(pen);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GoToWarehouse()
    {
        var warehouse = _townController.GetClosestBuilding(BuildingType.Warehouse, PosInt);
        ContinueOrderGoToTransform(warehouse);
    }

    private void ContinueOrderGoToTransform(Transform destination)
    {
        if (destination == null)
        {
            TownController.Instance.PushNewOrder(_villagerModel.CurrentOrderType);
            AddOrder(OrderType.Roam);
            return;
        }

        var position = destination.position;
        GoToOrderDestination(BitMath.RoundToInt((Vector2) position));
    }

    public void AddOrder(OrderType orderType)
    {
        _villagerModel.Orders.Enqueue(orderType);
    }

    public OrderType GetNextOrder()
    {
        return _villagerModel.Orders.Count > 0 ? _villagerModel.Orders.Dequeue() : OrderType.Null;
    }

    private void GoToOrderDestination(Vector2Int position)
    {
        var path = (List<Node>) PathfindingController.Instance.FindPath(
            BitMath.RoundToInt((Vector2) transform.position), position);

        if (path.Count > 0)
        {
            var pos = path.Last();
            _villagerModel.DestinationCell = _cellMapController.GetCell(pos.Position);
            // I'm removing the last node in the path because the villager looks better
            // if it stops right before its destination
            path.RemoveAt(path.Count - 1);
        }

        if (path.Count > 0)
        {
            _villagerModel.CurrentPath = path;
            _villagerStateController.CurrentVillagerState = VillagerState.GetToOrder;
            return;
        }

        _villagerStateController.CurrentVillagerState = VillagerState.ProcessOrder;
    }

    private Vector2Int GetRoamDestination()
    {
        Vector2Int pos;
        do
        {
            var deltax =
                Mathf.RoundToInt(Random.Range(-_villagerModel.MaxRoamRadius, _villagerModel.MaxRoamRadius));
            var deltay =
                Mathf.RoundToInt(Random.Range(-_villagerModel.MaxRoamRadius, _villagerModel.MaxRoamRadius));

            pos = new Vector2Int(
                Mathf.RoundToInt(transform.position.x) + deltax,
                Mathf.RoundToInt(transform.position.y) + deltay);
        } while (!_cellMapController.IsWalkable(pos));

        return BitMath.RoundToInt(pos);
    }

    public new VillagerData GetData()
    {
        return new VillagerData
        {
            Position = Cell.Position
        };
    }
}