using System;
using UnityEngine;

public class CellMapController : BitController<CellMapController>
{
    private Cell[,] _cellMap;
    private Vector2Int _size;

    private NodeMap _nodeMap;

    public Vector2Int LowerBoundary => _nodeMap.LowerBoundary;
    public Vector2Int UpperBoundary => _nodeMap.UpperBoundary;

    private void Awake()
    {
        _nodeMap = NodeMap.Instance;
        _nodeMap.RegisterCallback(NotificationType.OnNavigationMapIsBaked, OnNavigationMapIsBaked);
    }

    private void OnNavigationMapIsBaked()
    {
        var nodes = _nodeMap.Nodes;
        _size = new Vector2Int(nodes.GetLength(0), nodes.GetLength(1));

        _cellMap = new Cell[_size.x, _size.y];

        for (var i = 0; i < _size.x; i++)
        {
            for (var j = 0; j < _size.y; j++)
            {
                var cell = new Cell
                {
                    Node = nodes[i, j]
                };

                if (i > 0)
                {
                    var leftCell = _cellMap[i - 1, j];
                    cell.Left = leftCell;
                    leftCell.Right = cell;
                }

                if (j > 0)
                {
                    var downCell = _cellMap[i, j - 1];
                    cell.Down = downCell;
                    downCell.Up = cell;
                }

                _cellMap[i, j] = cell;
            }
        }
    }

    public Cell GetCell(Vector2 pos)
    {
        var intPos = BitMath.RoundToInt(pos);
        try
        {
            return _cellMap[
                intPos.x - LowerBoundary.x,
                intPos.y - LowerBoundary.y
            ];
        }
        catch (IndexOutOfRangeException)
        {
            //Debug.LogError("Tile not found at " + pos);
            return null;
        }
    }

    public bool IsWalkable(Vector2Int pos)
    {
        return GetCell(pos).IsWalkable();
    }
}