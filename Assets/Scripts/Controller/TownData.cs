using System;
using System.Collections.Generic;

[Serializable]
public class TownData
{
    public int VillagersInIsland;
    public int VillagersLeftTheIsland;
    public int CurrentDay;

    public Dictionary<BuildingType, List<BuildingData>> Buildings;
    public Dictionary<DropItemType, int> Storage;
    
    public List<VillagerData> Villagers;
}