#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CheatButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapSettingTool component = (MapSettingTool)target;
        if (GUILayout.Button("Generate Map"))
            component.OnClickedGenerateMapButton();

        if (GUILayout.Button("Delete Map"))
            component.OnClickDeleteMapButton();
    }
}
#endif