using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Forward Time Potion", 
    menuName = "ReWild/Items/Potions/Forward Time"
    )]
public class ForwardTimePotion : Potion
{
    public float MinutesToForward;

    protected override void ApplyEffect()
    {
        LegacyTimeController.Instance.AddTime(TimeSpan.FromMinutes(MinutesToForward));
    }
}