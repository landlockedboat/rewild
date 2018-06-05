using UnityEngine;

public class TownOrder
{
    public OrderType OrderType;
    public Cell Cell;

    public Vector2Int Position => Cell.Position;
    public uint Ammount;
}