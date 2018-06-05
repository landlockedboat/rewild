using UnityEngine;

public class CellObjectController : BitController
{
    public Cell Cell;
    public Vector2Int Position => Cell.Position;

    public CellObjectData GetData()
    {
        return new CellObjectData {Position = Position};
    }
}