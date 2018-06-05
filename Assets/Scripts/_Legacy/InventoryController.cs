using System.Collections.Generic;
using UnityEngine;

public class InventoryController : BitController<InventoryController>
{
    public Dictionary<string, Item> Inventory = new Dictionary<string, Item>();

     public void AddItem(Item item)
        {
            var itemName = item.Name;
            if (!Inventory.ContainsKey(itemName))
            {
                Inventory.Add(itemName, item);
            }
            
            ++Inventory[itemName].InventoryAmmount;
        }
    
        public void RemoveItem(Item item)
        {
            var itemName = item.Name;
            if (!Inventory.ContainsKey(itemName))
            {
                Debug.LogError("No item " + itemName + " in inventory");
                return;
            }
            --Inventory[itemName].InventoryAmmount;
        }
    
        public bool ItemExists(Item item)
        {
            var itemName = item.Name;
            if (Inventory.ContainsKey(itemName))
            {
                return Inventory[itemName].InventoryAmmount > 0;
            }
    
            return false;
        }
}