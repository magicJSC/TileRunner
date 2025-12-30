using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpawnSO", menuName = "Scriptable Objects/MonsterSpawnSO")]
public class MonsterSpawnSO : ScriptableObject
{
    public float spawnInterval = 2.0f;
    public List<GameObject> spawnList;
}

