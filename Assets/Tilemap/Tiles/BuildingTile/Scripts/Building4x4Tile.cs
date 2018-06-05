using System;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityEngine.Tilemaps
{
    [Serializable]
    public class Building4X4Tile : TileBase
    {


#if UNITY_EDITOR
        [MenuItem("Assets/Create/Building Tile")]
        public static void CreateBuildingTile()
        {
            var path = EditorUtility.SaveFilePanelInProject("Save Building Tile", "New Building Tile", "asset",
                "Save Building Tile", "Assets");

            if (path == "")
                return;

            AssetDatabase.CreateAsset(CreateInstance<AreaTile>(), path);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AreaTile))]
    public class BuildingTileEditor : Editor
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
                (Sprite) EditorGUILayout.ObjectField("Bot", Tile.MaskSprites[0], typeof(Sprite), false, null);
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