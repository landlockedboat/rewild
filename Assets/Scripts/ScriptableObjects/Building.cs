using UnityEngine;

[CreateAssetMenu(
    fileName = "New Building", 
    menuName = "ReWild/Building"
)]
public class Building : Item
{
    [HideInInspector]
    public Vector2Int Position;
    
    public override void Select()
    {
        CanvasController.Instance.SetInventoryUiActive(false);
        CanvasController.Instance.ShowBuildUi(this);
    }
    
    protected override void ApplyEffect()
    {
        BuildController.Instance.BuildBuilding(Position, this);
    }
}