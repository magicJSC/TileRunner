using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapDatabase : MonoBehaviour
{
    public static MapDatabase Instance { get; private set; }

    [Header("Start Map")]
    public HexMapSO startMap;

    [Header("All Maps")]
    public List<HexMapSO> mapList = new();

    [Header("Difficulty Thresholds")]
    public List<DifficultyWeight> difficultyTable = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// 점수에 따라 확률 기반으로 맵 선택
    /// </summary>
    public HexMapSO GetMapByScore(int score)
    {
        var tier = difficultyTable
            .LastOrDefault(t => score >= t.minScore);

        if (tier == null)
        {
            Debug.LogWarning("No difficulty tier found, returning startMap");
            return startMap;
        }

        int pickedDifficulty = PickByWeight(tier.weights);

        var candidates = mapList
            .Where(m => m.difficulty == pickedDifficulty)
            .ToList();

        if (candidates.Count == 0)
        {
            Debug.LogWarning($"No map found for difficulty {pickedDifficulty}");
            return startMap;
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

    /// <summary>
    /// 가중치 기반 난이도 선택
    /// </summary>
    private int PickByWeight(List<LevelWeight> weights)
    {
        float totalWeight = weights.Sum(w => w.weight);
        float rand = Random.Range(0f, totalWeight);

        float current = 0f;
        foreach (var w in weights)
        {
            current += w.weight;
            if (rand <= current)
                return w.difficulty;
        }

        return weights.Last().difficulty; // 안전장치
    }

#if UNITY_EDITOR
    public void RefreshMapList()
    {
        mapList.Clear();

        string[] guids = AssetDatabase.FindAssets("t:HexMapSO");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            HexMapSO map = AssetDatabase.LoadAssetAtPath<HexMapSO>(path);

            if (map != null)
                mapList.Add(map);
        }

        EditorUtility.SetDirty(this);
    }
#endif
}

[System.Serializable]
public class DifficultyWeight
{
    public int minScore;
    public List<LevelWeight> weights;
}

[System.Serializable]
public class LevelWeight
{
    public int difficulty;
    public float weight;
}
