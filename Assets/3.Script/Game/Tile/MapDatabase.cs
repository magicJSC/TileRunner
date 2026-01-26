using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapDatabase : MonoBehaviour
{
    public static MapDatabase Instance { get {  return instance; } set { instance = value; } }
    private static MapDatabase instance;

    public HexMapSO startMap;

    public List<HexMapSO> mapList;

     private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public HexMapSO GetMapByScore(int score)
    {
        int targetDifficulty = 1;
        var candidates = mapList
            .Where(m => m.difficulty == targetDifficulty)
            .ToList();

        return candidates[Random.Range(0, candidates.Count)];
    }
}
