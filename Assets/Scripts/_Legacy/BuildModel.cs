using UnityEngine;

public class BuildModel : BitModel<BuildModel>
{
    public GameObject BuildingPrefab;
    [HideInInspector]
    public Building CurrentBuilding;
}