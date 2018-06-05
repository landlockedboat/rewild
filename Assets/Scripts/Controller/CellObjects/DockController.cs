public class DockController : BuildingController
{
    public Cell SpawnCell => Cell.GetNeighbourOfType(TilemapType.Terrain);
}