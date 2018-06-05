public class BuildMission : Mission
{
    public BuildingType BuildingType;

    public override void Start()
    {
        CurrentAmmount = TownModel.Instance.Buildings[BuildingType].Count;
        SpawnController.Instance.RegisterCallback(NotificationType.OnBuildingBuilt,
            () =>
            {
                if (SpawnController.Instance.LastBuildingBuilt == BuildingType)
                {
                    ++CurrentAmmount;
                }
            });
        base.Start();
    }
}