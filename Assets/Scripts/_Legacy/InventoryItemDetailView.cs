using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDetailView : ItemDetailView
{
    [SerializeField] private Text _shopItemName;
    [SerializeField] private Text _inventoryItemAmmount;
    [SerializeField] private Image _shopItemImage;

    void Start()
    {
        _shopItemName.text = Item.Name;
        _shopItemImage.sprite = Item.DisplaySprite;
        UpdateItemAmmountText();
    }

    private void UpdateItemAmmountText()
    {
        _inventoryItemAmmount.text = Item.InventoryAmmount.ToString();
    }

    public void SelectItem()
    {
        Item.Select();
        UpdateItemAmmountText();
    }
}