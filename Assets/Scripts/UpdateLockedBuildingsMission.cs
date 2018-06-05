public class UpdateLockedBuildingsMission : InfoMission
{
    public BuildingType[] BuildingsToUnlock;
    public BuildingType[] BuildingsToLock;

    public override void Start()
    {
        var buildings = LevelConfiguration.Instance.AvailableBuildings;
        buildings.AddRange(BuildingsToUnlock);

        if (BuildingsToLock != null)
        {
            foreach (var building in BuildingsToLock)
            {
                if (buildings.Contains(building))
                {
                    buildings.Remove(building);
                }
            }
        }

        base.Start();
    }
}