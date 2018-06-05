public class BuildingController : CellObjectController
{
    public BuildingType BuildingType;

    public new BuildingData GetData()
    {
        return new BuildingData
        {
            BuildingType = BuildingType,
            Position = Position
        };
    }
}    
