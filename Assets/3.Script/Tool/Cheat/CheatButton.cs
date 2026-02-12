#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheatTool))]
public class CheatButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CheatTool component = (CheatTool)target;
        if (GUILayout.Button("Add Money"))
            component.OnClickedAddMoneyButton();
    }
}
#endif