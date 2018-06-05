using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "ReWild/Items/Plant")]
public class Plant : Building
{
    public float MinutesToGrow;
    public Sprite GrowSprite;
    public int GrowReward;


}
