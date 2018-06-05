using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : BitController<SpawnController>
{
    private CellMapController _cellMapController;
    private SpawnModel _spawnModel;
    private TownModel _townModel;

    [HideInInspector] public BuildingType LastBuildingBuilt;
    [HideInInspector] public BuildingType LastBuildingBulldozed;

    private void Awake()
    {
        _townModel = TownModel.Instance;
        _spawnModel = SpawnModel.Instance;
    }

    private void Start()
    {
        _cellMapController = CellMapController.Instance;
    }

    private GameObject SpawnCellObject(GameObject prefab, Vector2Int vector2Int)
    {
        var instance = Instantiate(prefab, (Vector2) vector2Int, Quaternion.identity);
        var controller = instance.GetComponent<CellObjectController>();
        var cell = _cellMapController.GetCell(vector2Int);

        controller.Cell = cell;
        cell.CellObjects.Add(controller);
        return instance;
    }

    private GameObject SpawnObject(GameObject spawnObject, Vector2Int position)
    {
        return SpawnCellObject(spawnObject, position);
    }

    public void SpawnBuilding(BuildingType type, Vector2Int position)
    {
        GameObject spawnedGameObject;
        switch (type)
        {
            case BuildingType.House:
                spawnedGameObject = SpawnObject(_spawnModel.HousePrefab, position);
                break;
            case BuildingType.Warehouse:
                spawnedGameObject = SpawnObject(_spawnModel.WarehousePrefab, position);
                break;
            case BuildingType.Oven:
                spawnedGameObject = SpawnObject(_spawnModel.OvenPrefab, position);
                break;
            case BuildingType.Slaughterhouse:
                spawnedGameObject = SpawnObject(_spawnModel.SlaughterhousePrefab, position);
                break;
            case BuildingType.Pen:
                spawnedGameObject = SpawnObject(_spawnModel.PenPrefab, position);
                break;
            case BuildingType.TofuFermenter:
                spawnedGameObject = SpawnObject(_spawnModel.TofuFermenterPrefab, position);
                break;
            case BuildingType.TofuFarm:
                spawnedGameObject = SpawnFarm(BuildingType.TofuFarm, position);
                break;
            case BuildingType.WheatFarm:
                spawnedGameObject = SpawnFarm(BuildingType.WheatFarm, position);
                break;
            case BuildingType.Dock:
                spawnedGameObject = SpawnObject(_spawnModel.DockPrefab, position);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        LastBuildingBuilt = type;
        var controller = spawnedGameObject.GetComponent<BuildingController>();
        _townModel.Buildings[type].Add(controller);
        TriggerCallback(NotificationType.OnBuildingBuilt);
    }

    public VillagerController SpawnVillager()
    {
        var dockCell = _townModel.Dock.SpawnCell;
        return SpawnVillager(dockCell.Position);
    }

    public VillagerController SpawnVillager(Vector2Int position)
    {
        var villager = SpawnCellObject(_spawnModel.VillagerPrefab, position);

        var sr = villager.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        var randIndex = Mathf.FloorToInt(Random.value * _spawnModel.VillagerSprites.Length);
        sr.sprite = _spawnModel.VillagerSprites[randIndex];

        var villagerController = villager.GetComponent<VillagerController>();
        TownModel.Instance.Villagers.Add(villagerController);

        TownController.Instance.VillagersInIsland = TownModel.Instance.Villagers.Count;
        return villagerController;
    }

    private GameObject SpawnFarm(BuildingType buildingType, Vector2Int pos)
    {
        TilemapController.Instance.SetTile(TilemapType.Farm, _townModel.FarmTile, BitMath.ToVector3Int(pos));
        
        var farm = SpawnObject(_spawnModel.FarmPrefab, pos).GetComponent<FarmController>();
        farm.BuildingType = buildingType;
        return farm.gameObject;
    }

    public CropController SpawnCrop(GameObject prefab, Vector2Int position)
    {
        if (!TilemapController.Instance.HasTile(TilemapType.Farm, BitMath.ToVector3Int(position)))
        {
            Debug.LogError("Cannot plant here!");
            return null;
        }

        var crop = SpawnCellObject(prefab, position).GetComponent<CropController>();
        return crop;
    }

    public void SpawnWheatCrop(Vector2Int position)
    {
        var crop = SpawnCrop(_spawnModel.WheatCropGameObject, position);
        _townModel.WheatPlants.Add(crop);
    }

    public void SpawnTofuCrop(Vector2Int position)
    {
        var crop = SpawnCrop(_spawnModel.TofuCropGameObject, position);
        _townModel.TofuPlants.Add(crop);
    }

    public void SpawnDroppedItem(DropItemType type, Vector2Int position, int producedItemAmmount = 1)
    {
        GameObject prefab;
        switch (type)
        {
            case DropItemType.Wheat:
                prefab = _spawnModel.WheatItemPrefab;
                break;
            case DropItemType.Bread:
                prefab = _spawnModel.BreadItemPrefab;
                break;
            case DropItemType.Tofu:
                prefab = _spawnModel.TofuItemPrefab;
                break;
            case DropItemType.Soybean:
                prefab = _spawnModel.SoybeanItemPrefab;
                break;
            case DropItemType.Piggy:
                prefab = _spawnModel.PiggyItemPrefab;
                break;
            case DropItemType.Meat:
                prefab = _spawnModel.MeatItemPrefab;
                break;
            case DropItemType.None:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        var item = SpawnCellObject(prefab, position).GetComponent<DroppedItemController>();
        item.Ammount = producedItemAmmount;
        _townModel.DroppedItems.Add(item);
    }
    
    public bool DestroyBuilding(Vector2Int bulldozePos)
    {
        var cell = CellMapController.Instance.GetCell(bulldozePos);

        if (cell == null)
        {
            return false;
        }

        if (DestroyFarmController(cell))
        {
            return true;
        }

        var building = cell.GetCellObject<BuildingController>();

        if (building == null)
        {
            return false;
        }

        _townModel.Buildings[building.BuildingType].Remove(building);
        Destroy(building.gameObject);
        LastBuildingBulldozed = building.BuildingType;
        TriggerCallback(NotificationType.OnBuildingBulldozed);
        return true;
    }

    private bool DestroyFarmController(Cell cell)
    {
        var farm = cell.GetCellObject<FarmController>();

        if (farm != null)
        {
            TilemapController.Instance.DeleteTile(TilemapType.Farm,
                BitMath.ToVector3Int(cell.Position));


            var crop = cell.GetCellObject<CropController>();
            if (crop != null)
            {
                switch (crop.BuildingType)
                {
                    case BuildingType.WheatFarm:
                        _townModel.WheatPlants.Remove(crop);
                        break;
                    case BuildingType.TofuFarm:
                        _townModel.TofuPlants.Remove(crop);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Destroy(crop.gameObject);
            }
        }
        else
        {
            return false;
        }

        _townModel.Buildings[farm.BuildingType].Remove(farm);
        Destroy(farm.gameObject);
        return true;
    }
}