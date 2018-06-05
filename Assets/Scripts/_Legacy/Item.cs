using UnityEngine;

public abstract class Item : ScriptableObject
{
    private string _translatedName;

    public string Name
    {
        get
        {
            if (string.IsNullOrEmpty(_translatedName))
            {
                _translatedName = LocalisationManager.Instance.GetTranslation(name.ToLower());
            }

            return _translatedName;
        }
    }
    
    public int Price;
    public Sprite DisplaySprite;
    [HideInInspector]
    public int InventoryAmmount;

    public virtual void Select() { }

    public bool Consume()
    {
        if (!InventoryController.Instance.ItemExists(this))
        {
            return false;
        }
        InventoryController.Instance.RemoveItem(this);
        ApplyEffect();
        return true;
    }

    protected virtual void ApplyEffect() { }
}