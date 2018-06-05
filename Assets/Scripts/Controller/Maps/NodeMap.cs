using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class NodeMap : BitController<NodeMap>
{
    private bool _isBaked;

    public bool IsBaked
    {
        get { return _isBaked; }
        private set
        {
            if (value)
            {
                TriggerCallback(NotificationType.OnNavigationMapIsBaked);
            }

            _isBaked = value;
        }
    }

    private string _nodesPath;
    private Vector2Int TerrainSize => _tilemapController.TerrainSize;

    private TilemapController _tilemapController;

    public Vector2Int LowerBoundary => _tilemapController.LowerBoundary;
    public Vector2Int UpperBoundary => _tilemapController.UpperBoundary;

    public Node[,] Nodes { get; private set; }

    [Serializable]
    private class SerializableNodes
    {
        public Node[] Nodes;
    }

    private void InitializeAttributes()
    {
        _tilemapController = TilemapController.Instance;
        _nodesPath = Path.Combine(Application.persistentDataPath,
            SceneManager.GetActiveScene().name + "_navigationMap.json");
    }

    public void BakeNavigationMap()
    {
        IsBaked = false;
        InitializeAttributes();

        CreateNodes();

        SaveNodes();

        IsBaked = true;
    }

    private void CreateNodes()
    {
        Nodes = new Node[TerrainSize.x, TerrainSize.y];

        for (var x = LowerBoundary.x; x < UpperBoundary.x; x++)
        {
            for (var y = LowerBoundary.y; y < UpperBoundary.y; y++)
            {
                CreateNode(new Vector2Int(x, y));
            }
        }
    }

    private void CreateNode(Vector2Int pos)
    {
        var v3 = new Vector3Int(pos.x, pos.y, 0);
        var walkable = _tilemapController.IsAccessible(v3);

        var newNode = new Node
        {
            Position = pos,
            Walkable = walkable
        };

        SetNode(pos, newNode);
    }

    private void SaveNodes()
    {
        var flattened = new Node[TerrainSize.x * TerrainSize.y];

        for (var x = 0; x < TerrainSize.x; x++)
        {
            for (var y = 0; y < TerrainSize.y; y++)
            {
                flattened[TerrainSize.y * x + y] = Nodes[x, y];
            }
        }

        var serializable = new SerializableNodes {Nodes = flattened};
        var json = JsonUtility.ToJson(serializable, false);
        File.WriteAllText(_nodesPath, json);
    }

    private bool LoadNodes()
    {
        if (!File.Exists(_nodesPath)) return false;
        var nodesText = File.ReadAllText(_nodesPath);
        var nodes = JsonUtility.FromJson<SerializableNodes>(nodesText).Nodes;

        Nodes = new Node[TerrainSize.x, TerrainSize.y];

        for (var x = 0; x < TerrainSize.x; x++)
        {
            for (var y = 0; y < TerrainSize.y; y++)
            {
                Nodes[x, y] = nodes[TerrainSize.y * x + y];
            }
        }
        return true;
    }

    private void Start()
    {
        InitializeAttributes();
        IsBaked = LoadNodes();
        if (!IsBaked)
        {
            BakeNavigationMap();
        }
    }

    public IEnumerable<Node> GetNeighbours(Node node)
    {
        var neighbours = new List<Node>();

        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                var neighbourPos = new Vector2Int(
                    node.Position.x + x,
                    node.Position.y + y
                );

                if (neighbourPos.x > LowerBoundary.x && neighbourPos.x < UpperBoundary.x &&
                    neighbourPos.y > LowerBoundary.y && neighbourPos.y < UpperBoundary.y)
                {
                    neighbours.Add(GetNode(neighbourPos));
                }
            }
        }

        return neighbours;
    }

    public Node GetNode(Vector2 pos)
    {
        var intPos = BitMath.RoundToInt(pos);
        try
        {
            return Nodes[
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

    public bool IsWalkable(Vector2 pos)
    {
        return GetNode(pos).Walkable;
    }

    private void SetNode(Vector2 pos, Node node)
    {
        var intPos = BitMath.RoundToInt(pos);
        Nodes[
            intPos.x - LowerBoundary.x,
            intPos.y - LowerBoundary.y
        ] = node;
    }

    private void OnDrawGizmosSelected()
    {
        if (!IsBaked) return;
        for (var x = LowerBoundary.x; x < UpperBoundary.x; x++)
        {
            for (var y = LowerBoundary.y; y < UpperBoundary.y; y++)
            {
                if (Nodes == null) return;
                var pos = new Vector2(x, y);
                var node = GetNode(pos);

                Gizmos.color = node.Walkable ? Color.green : Color.red;
                Gizmos.DrawWireCube(new Vector2(x, y) + Vector2.one * .5f, Vector3.one);
            }
        }
    }
}