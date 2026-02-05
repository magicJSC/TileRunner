#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapDatabase))]
public class RefreshMapButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapDatabase component = (MapDatabase)target;
        if (GUILayout.Button("Refresh Map SO"))
            component.RefreshMapList();
    }
}
#endif