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
    public AssetReferenceHexMapSO startMapRef;
    public HexMapSO startMapFallback;

    [Header("All Maps")]
    public List<AssetReferenceHexMapSO> mapReferences = new();
    public List<HexMapSO> mapFallbackList = new();

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
            Debug.LogWarning("난이도 테이블이 없습니다");
            return GetStartMap();
        }

        int pickedDifficulty = PickByWeight(tier.weights);

        var candidates = GetAllMaps()
            .Where(m => m != null && m.difficulty == pickedDifficulty)
            .ToList();

        if (candidates.Count == 0)
        {
            Debug.LogWarning($"난이도에 맞는 맵이 없습니다 : {pickedDifficulty}");
            return GetStartMap();
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

    public HexMapSO GetStartMap()
    {
        // Addressable 우선
        if (IsAddressableReady())
        {
            var map = AddressableManager.Instance.Get<HexMapSO>(startMapRef);
            if (map != null)
                return map;
        }

        // Fallback
        return startMapFallback;
    }

    private IEnumerable<HexMapSO> GetAllMaps()
    {
        // Addressable 사용 가능
        if (IsAddressableReady())
        {
            foreach (var reference in mapReferences)
            {
                var map = AddressableManager.Instance.Get<HexMapSO>(reference);
                if (map != null)
                    yield return map;
            }
        }
        else
        {
            // 에디터 / 개발 중 fallback
            foreach (var map in mapFallbackList)
            {
                if (map != null)
                    yield return map;
            }
        }
    }

    private bool IsAddressableReady()
    {
        return AddressableManager.Instance != null
               && AddressableManager.Instance.IsReady;
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

        return weights.Last().difficulty;
    }

#if UNITY_EDITOR
    public void RefreshMapList()
    {
        mapReferences.Clear();
        mapFallbackList.Clear();

        string startGuid = startMapRef != null
            ? startMapRef.AssetGUID
            : string.Empty;

        string[] guids = AssetDatabase.FindAssets("t:HexMapSO");

        foreach (string guid in guids)
        {
            if (!string.IsNullOrEmpty(startGuid) && guid == startGuid)
                continue;

            string path = AssetDatabase.GUIDToAssetPath(guid);
            HexMapSO map = AssetDatabase.LoadAssetAtPath<HexMapSO>(path);

            if (map == null)
                continue;

            // Addressable용
            mapReferences.Add(new AssetReferenceHexMapSO(guid));

            // Fallback용
            mapFallbackList.Add(map);
        }

        EditorUtility.SetDirty(this);
        Debug.Log($"[MapDatabase] Maps refreshed : {mapReferences.Count}");
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
