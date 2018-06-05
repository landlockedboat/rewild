using System;
using UnityEngine;

public class BuildController : BitController<BuildController>
{
    private BuildModel _buildModel;

    private void Awake()
    {
        _buildModel = BuildModel.Instance;
    }

    public void ConsumeCurrentBuilding()
    {
        _buildModel.CurrentBuilding.Consume();
    }

    public void BuildBuilding(Vector2 position, Building building)
    {
        throw new NotImplementedException();
        //var go = Instantiate(_buildModel.BuildingPrefab, position, Quaternion.identity);
        //var buildingController = go.GetComponent<BuildingController>();
        // buildingController.Building = building;
        //TriggerCallback(NotificationType.BuildingBuilt);
    }
}