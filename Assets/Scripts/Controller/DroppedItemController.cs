using UnityEngine;

public class DroppedItemController : CellObjectController
{
    public DropItemType Type;
    public int Ammount = 1;

    private void Start()
    {
        TownController.Instance.PushNewOrder(OrderType.PickItem, BitMath.RoundToInt((Vector2) transform.position));
        Instantiate(SpawnModel.Instance.DroppedItemAmmountPrefab, transform);
    }
}