using UnityEngine;
using UnityEngine.UI;

public class ShopItemDetailView : ItemDetailView
{
    [SerializeField] private Text _shopItemName;
    [SerializeField] private Text _shopItemPrice;
    [SerializeField] private Image _shopItemImage;

    void Start()
    {
        _shopItemName.text = Item.Name;
        _shopItemPrice.text = Item.Price + " G";
        _shopItemImage.sprite = Item.DisplaySprite;
    }

    public void BuyItem()
    {
        if (PlayerModel.Instance.Money < Item.Price)
        {
            return;
        }

        PlayerController.Instance.AddMoney(-Item.Price);
        InventoryController.Instance.AddItem(Item);
    }
}