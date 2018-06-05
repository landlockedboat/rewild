using UnityEngine;

[CreateAssetMenu(
    fileName = "New Money Potion",
    menuName = "ReWild/Items/Potions/Money"
)]
public class MoneyPotion : Potion
{
    public int MoneyToAdd;

    protected override void ApplyEffect()
    {
        PlayerController.Instance.AddMoney(MoneyToAdd);
    }
}
