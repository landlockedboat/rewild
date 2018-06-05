using System.Linq;

public class BulldozeBuildingsMission : Mission
{
    public BuildingType[] BuildingTypesToBulldoze;
    
    public override void Start()
    {
        CurrentAmmount = 0;
        ObjectiveAmmount = GetBuildingsToDestroyAmmount();
        
        SpawnController.Instance.RegisterCallback(NotificationType.OnBuildingBulldozed, OnBuildingBulldozed);
        base.Start();
    }

    private void OnBuildingBulldozed()
    {
        var type = SpawnController.Instance.LastBuildingBulldozed;
        if (BuildingTypesToBulldoze.Contains(type))
        {
            ++CurrentAmmount;
        }
    }

    private int GetBuildingsToDestroyAmmount()
    {
        return BuildingTypesToBulldoze.Sum(buildingType => TownModel.Instance.Buildings[buildingType].Count);
    }
}