using System;
using System.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityEngine.Tilemaps
{
    [Serializable]
    public class AreaTile : TileBase
    {
        [SerializeField] public Sprite[] MaskSprites;

        public override void RefreshTile(Vector3Int location, ITilemap tileMap)
        {
            for (var yd = -1; yd <= 1; yd++)
            for (var xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (TileValue(tileMap, position))
                    tileMap.RefreshTile(position);
            }
        }

        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            UpdateTile(location, tileMap, ref tileData);
        }

        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;

            var mask = new BitArray(8)
            {
                [0] = TileValue(tileMap, location + new Vector3Int(1, 1, 0)),
                [1] = TileValue(tileMap, location + new Vector3Int(0, 1, 0)),
                [2] = TileValue(tileMap, location + new Vector3Int(-1, 1, 0)),
                [3] = TileValue(tileMap, location + new Vector3Int(-1, 0, 0)),
                [4] = TileValue(tileMap, location + new Vector3Int(-1, -1, 0)),
                [5] = TileValue(tileMap, location + new Vector3Int(0, -1, 0)),
                [6] = TileValue(tileMap, location + new Vector3Int(1, -1, 0)),
                [7] = TileValue(tileMap, location + new Vector3Int(1, 0, 0)),
            };

            var neighbours = mask.OfType<bool>().Count(p => p);
            var index = GetIndex(neighbours, mask);

            //Debug
            //tileData.color = GetColor(neighbours);

            if (index < 0 || index >= MaskSprites.Length || !TileValue(tileMap, location)) return;
            tileData.sprite = MaskSprites[index];
            tileData.transform = GetTransform(index, mask);
            tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
            tileData.colliderType = Tile.ColliderType.Grid;
        }

        private Color GetColor(int neighbours)
        {
            switch (neighbours)
            {
                case 1: return Color.blue;
                case 2: return Color.cyan;
                case 3: return Color.gray;
                case 4: return Color.green;
                case 5: return Color.magenta;
                case 6: return Color.red;
                case 7: return Color.yellow;
                case 8: return Color.white;
                default: return Color.black;
            }
        }

        private bool TileValue(ITilemap tileMap, Vector3Int position)
        {
            TileBase tile = tileMap.GetTile(position);
            return (tile != null && tile == this);
        }


        private int GetIndex(int neighbours, BitArray mask)
        {
            switch (neighbours)
            {
                case 4:
                case 3: return 2;
                case 6:
                case 5:
                    if (HasCorner(mask))
                    {
                        return 2;
                    }
                    return 1;
                case 7: return 3;
                case 8: return 0;
                default:
                    return 0;
            }
        }

        private bool HasCorner(BitArray mask)
        {
            return (!mask[1] && !mask[0] && !mask[7]) ||
                   (!mask[7] && !mask[6] && !mask[5]) ||
                   (!mask[5] && !mask[4] && !mask[3]) ||
                   (!mask[3] && !mask[2] && !mask[1]);
        }

        private Matrix4x4 GetTransform(int index, BitArray mask)
        {
            switch (index)
            {
                // Center
                case 0:
                    break;
                // Side
                case 1:
                    if (!mask[1])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90f));
                    }

                    if (!mask[7])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, -180f));
                    }

                    if (!mask[5])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90f));
                    }

                    break;
                //Corner
                case 2:
                    if (!mask[1] && !mask[3])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90f));
                    }

                    if (!mask[1] && !mask[7])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180f));
                    }

                    if (!mask[5] && !mask[7])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90f));
                    }

                    break;
                // ICorner
                case 3:
                    if (!mask[0])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180f));
                    }

                    if (!mask[2])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90f));
                    }

                    if (!mask[6])
                    {
                        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90f));
                    }

                    break;
            }

            return Matrix4x4.identity;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Area Tile")]
        public static void CreateAreaTile()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Area Tile", "New Area Tile", "asset",
                "Save Area Tile", "Assets");

            if (path == "")
                return;

            AssetDatabase.CreateAsset(CreateInstance<AreaTile>(), path);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AreaTile))]
    public class AreaTileEditor : Editor
    {
        private AreaTile Tile => (target as AreaTile);
        private const int NumberOfSprites = 4;

        public void OnEnable()
        {
            if (Tile.MaskSprites == null || Tile.MaskSprites.Length != NumberOfSprites)
                Tile.MaskSprites = new Sprite[NumberOfSprites];
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place the sprites according to the area's boundaries.");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            Tile.MaskSprites[0] =
                (Sprite) EditorGUILayout.ObjectField("Center", Tile.MaskSprites[0], typeof(Sprite), false, null);
            Tile.MaskSprites[1] =
                (Sprite) EditorGUILayout.ObjectField("Side", Tile.MaskSprites[1], typeof(Sprite), false, null);
            Tile.MaskSprites[2] =
                (Sprite) EditorGUILayout.ObjectField("Corner", Tile.MaskSprites[2], typeof(Sprite), false, null);
            Tile.MaskSprites[3] =
                (Sprite) EditorGUILayout.ObjectField("Inverse corner", Tile.MaskSprites[3], typeof(Sprite), false,
                    null);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(Tile);
        }
    }
#endif
}