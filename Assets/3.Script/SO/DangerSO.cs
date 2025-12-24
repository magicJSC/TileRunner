using UnityEngine;

[CreateAssetMenu(fileName = "DangerSO", menuName = "Scriptable Objects/DangerSO")]
public class DangerSO : ScriptableObject
{
    public int levelIndex;
    public int removeCount = 3;
    public bool fog = false;
    public int tornadoCount = 1;
}
