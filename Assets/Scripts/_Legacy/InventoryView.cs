using System.Linq;

public class InventoryView : ItemDisplayView
{
    private void OnEnable()
    {
        ItemsToDisplay = InventoryController.Instance.Inventory.Values.ToArray();
        DisplayItems();
    }
}