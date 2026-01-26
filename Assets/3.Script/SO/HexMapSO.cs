using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexMapSO", menuName = "Scriptable Objects/HexMapSO")]
public class HexMapSO : ScriptableObject
{
    public int difficulty;
    public List<TileInfo> tiles;
}
