public class BuildingData : CellObjectData
{
    public BuildingType BuildingType;

    public override void Spawn()
    {
        SpawnController.Instance.SpawnBuilding(BuildingType, Position);
    }
}