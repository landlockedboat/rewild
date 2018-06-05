using System.Collections.Generic;

public class InventoryModel : Singleton<InventoryModel>
{
    public readonly Dictionary<string, Item> Inventory = new Dictionary<string, Item>();    
}