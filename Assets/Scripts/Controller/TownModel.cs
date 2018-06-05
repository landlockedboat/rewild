using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TownModel : BitModel<TownModel>
{
    public TileBase FarmTile;
    public int MaxVillagersLeaving = 3;

    [HideInInspector] public List<CropController> WheatPlants;
    [HideInInspector] public List<CropController> TofuPlants;
    
    [HideInInspector] public List<DroppedItemController> DroppedItems;
    [HideInInspector] public List<VillagerController> Villagers;

    [HideInInspector] public Dictionary<BuildingType, List<BuildingController>> Buildings =
        new Dictionary<BuildingType, List<BuildingController>>();

    [HideInInspector] public int VillagersInIsland;
    [HideInInspector] public int VillagersLeftTheIsland;

    [HideInInspector] public Queue<TownOrder> TownOrders;

    [HideInInspector] public Dictionary<DropItemType, int> Storage = new Dictionary<DropItemType, int>();

    public DockController Dock => (DockController)Buildings[BuildingType.Dock].First();

    private void Awake()
    {
        foreach (DropItemType type in Enum.GetValues(typeof(DropItemType)))
        {
            Storage.Add(type, 0);
        }

        foreach (BuildingType type in Enum.GetValues(typeof(BuildingType)))
        {
            Buildings.Add(type, new List<BuildingController>());
        }
    }

    public void FromData(TownData townData)
    {
        VillagersInIsland = townData.VillagersInIsland;
        VillagersLeftTheIsland = townData.VillagersLeftTheIsland;
        Storage = townData.Storage;

        TimeController.Instance.CurrentDay = townData.CurrentDay;

        foreach (var buildingList in townData.Buildings)
        {
            foreach (var building in buildingList.Value)
            {
                
                building.Spawn();
            }
        }

        foreach (var villager in townData.Villagers)
        {
            SpawnController.Instance.SpawnVillager(villager.Position);
        }

        StartCoroutine(TriggerCallbacks());
    }

    public TownData ToData()
    {
        var townData = new TownData
        {
            VillagersInIsland = VillagersInIsland,
            VillagersLeftTheIsland = VillagersLeftTheIsland,
            Villagers = Villagers.Select(v => v.GetData()).ToList(),
            Buildings = new Dictionary<BuildingType, List<BuildingData>>(),
            Storage = Storage
        };

        foreach (var buildingList in Buildings)
        {
            townData.Buildings.Add(buildingList.Key, new List<BuildingData>());

            foreach (var building in buildingList.Value)
            {
                townData.Buildings[buildingList.Key].Add(building.GetData());
            }
        }
        
        townData.CurrentDay = TimeController.Instance.CurrentDay;
        return townData;
    }
    
       
    private IEnumerator TriggerCallbacks()
    {
        // Wait 1 frame
        yield return 0;
        TownController.Instance.TriggerCallback(NotificationType.OnWarehouseStorageChanged);
        TownController.Instance.TriggerCallback(NotificationType.OnVillagerAmmountChanged);
        TimeController.Instance.TriggerCallback(NotificationType.OnDayPassed);
    }
}