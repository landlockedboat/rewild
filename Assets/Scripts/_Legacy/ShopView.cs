using System.IO;
using UnityEngine;

public class ShopView : ItemDisplayView
{
    private void Awake()
    {
        var itemsPath = Path.Combine("Items", "Shop");
        ItemsToDisplay = Resources.LoadAll<Item>(itemsPath);
        DisplayItems();
    }
}