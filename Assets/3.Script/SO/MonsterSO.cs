using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSO", menuName = "Scriptable Objects/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    public GameObject monsterPrefab;
    public Vector3 spawnPos;
}
