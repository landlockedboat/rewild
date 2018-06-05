using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Node Node;

    public Cell Up;
    public Cell Down;
    public Cell Left;
    public Cell Right;

    private List<Cell> _neighbours;
    public Vector2Int Position => Node.Position;

    public readonly List<CellObjectController> CellObjects = new List<CellObjectController>();

    public T GetCellObject<T>() where T : CellObjectController
    {
        foreach (var cellObject in CellObjects)
        {
            try
            {
                return (T) cellObject;
            }
            catch (InvalidCastException)
            {
            }
        }

        return null;
    }

    public Cell GetNeighbourOfType(TilemapType tilemapType)
    {
        _neighbours = new List<Cell> {Up, Down, Left, Right};
        foreach (var neighbour in _neighbours)
        {
            if (TilemapController.Instance.HasTile(tilemapType, BitMath.ToVector3Int(neighbour.Position)))
            {
                return neighbour;
            }
        }

        Debug.LogError("No neighbour of type " + tilemapType + " in tile " + Position);
        return null;
    }

    public Vector3 GetWorldPosition()
    {
        return new Vector3(Node.Position.x, Node.Position.y, 0);
    }

    public bool IsWalkable()
    {
        var walkable = Node.Walkable;
        walkable &= GetCellObject<BuildingController>() == null;
        return walkable;
    }
}