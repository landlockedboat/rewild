using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeMap))]
public class NavigationMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var navigationMap = (NodeMap) target;
        if (GUILayout.Button("Bake navigation map"))
        {
            navigationMap.BakeNavigationMap();
        }
    }
}