using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultSO", menuName = "Scriptable Objects/DifficultSO")]
public class DifficultSO : ScriptableObject
{
    public int minScore;
    public List<LevelWeight> weights;
}
