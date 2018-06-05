using System;

public class FarmController : BuildingController
{
    private void Start()
    {
        PlantCrop();
    }

    private void PlantCrop()
    {
        OrderType orderType;
        switch (BuildingType)
        {
            case BuildingType.TofuFarm:
                orderType = OrderType.PlantTofu;
                break;
            case BuildingType.WheatFarm:
                orderType = OrderType.PlantWheat;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        TownController.Instance.PushNewOrder(orderType, Cell.Position);
    }
}