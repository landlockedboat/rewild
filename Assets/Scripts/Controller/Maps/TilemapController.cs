using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class TilemapController : BitController<TilemapController>
{
    [SerializeField] private Tilemap _buildingTilemap;
    [SerializeField] private Tilemap _farmTilemap;
    [SerializeField] private Tilemap _terrainTilemap;
    [SerializeField] private Tilemap _seaTilemap;

    private Vector2Int _terrainSize = Vector2Int.zero;

    private Dictionary<TilemapType, Tilemap> _tilemaps;

    public Vector2Int TerrainSize
    {
        get { return _terrainSize; }
    }

    public Vector2Int LowerBoundary;
    public Vector2Int UpperBoundary;

    private void OnEnable()
    {
        LowerBoundary = new Vector2Int(int.MaxValue, int.MaxValue);
        UpperBoundary = new Vector2Int(int.MinValue, int.MinValue);

        _tilemaps = new Dictionary<TilemapType, Tilemap>
        {
            {TilemapType.Building, _buildingTilemap},
            {TilemapType.Farm, _farmTilemap},
            {TilemapType.Terrain, _terrainTilemap},
            {TilemapType.Sea, _seaTilemap}
        };

        foreach (var tilemapPair in _tilemaps)
        {
            var tilemap = tilemapPair.Value;
            if (_terrainSize.x < tilemap.size.x)
            {
                _terrainSize.x = tilemap.size.x;
            }

            if (_terrainSize.y < tilemap.size.y)
            {
                _terrainSize.y = tilemap.size.y;
            }

            var minBounds = tilemap.localBounds.min;
            var maxBounds = tilemap.localBounds.max;

            if (minBounds.x < LowerBoundary.x)
            {
                LowerBoundary.x = Mathf.RoundToInt(minBounds.x);
            }
            if (minBounds.y < LowerBoundary.y)
            {
                LowerBoundary.y = Mathf.RoundToInt(minBounds.y);
            }
            
            if (maxBounds.x > UpperBoundary.x)
            {
                UpperBoundary.x = Mathf.RoundToInt(maxBounds.x);
            }
            if (maxBounds.y > UpperBoundary.y)
            {
                UpperBoundary.y = Mathf.RoundToInt(maxBounds.y);
            }
        }
    }

    public bool IsAccessible(Vector3Int position)
    {
        foreach (var tilemapPair in _tilemaps)
        {
            if (!HasTile(tilemapPair.Key, position))
            {
                continue;
            }

            var tilemap = tilemapPair.Value;
            var colliderType = tilemap.GetColliderType(position);

            if (colliderType == Tile.ColliderType.None)
            {
                continue;
            }

            return false;
        }

        return true;
    }

    public void SetTile(TilemapType tilemapType, TileBase tile, Vector3Int position)
    {
        _tilemaps[tilemapType].SetTile(position, tile);
    }
    
    public void DeleteTile(TilemapType tilemapType, Vector3Int position)
    {
        _tilemaps[tilemapType].SetTile(position, null);
    }

    public bool HasTile(TilemapType tilemapType, Vector3Int position)
    {
        return _tilemaps[tilemapType].GetTile(position) != null;
    }
}

public enum TilemapType
{
    Building,
    Farm,
    Terrain,
    Sea
}