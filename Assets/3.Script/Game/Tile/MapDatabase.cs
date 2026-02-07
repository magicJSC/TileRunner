using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEngine.AddressableAssets;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapDatabase : MonoBehaviour
{
    public static MapDatabase Instance { get; private set; }

    [Header("Start Map")]
    public string startMapName;
    public AssetReference startMapRef;
    public HexMapSO startMapFallback;

    [Header("All Maps")]
    public List<string> mapNames = new();
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
            var map = AddressableManager.Instance.Get<HexMapSO>(startMapName);
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
            foreach (var reference in mapNames)
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
        mapNames.Clear();
        mapFallbackList.Clear();
        startMapName = string.Empty;

        // 1. Start Map 정보만 따로 추출해서 변수에 저장
        if (startMapRef != null)
        {
            string startPath = AssetDatabase.GUIDToAssetPath(startMapRef.AssetGUID);
            HexMapSO startMap = AssetDatabase.LoadAssetAtPath<HexMapSO>(startPath);

            if (startMap != null)
            {
                startMapName = startMap.name; 
            }
        }

        // 2. 프로젝트 내 모든 HexMapSO 검색 (중복 없이 리스트 채우기)
        string[] guids = AssetDatabase.FindAssets("t:HexMapSO");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            HexMapSO map = AssetDatabase.LoadAssetAtPath<HexMapSO>(path);

            if (map == null) continue;

            // 이미 리스트에 들어있다면 (방금 넣은 StartMap인 경우 등) 스킵
            if (mapNames.Contains(map.name) || map.name == startMapName)
                continue;

            mapNames.Add(map.name);
            mapFallbackList.Add(map);
        }

        EditorUtility.SetDirty(this);
        Debug.Log($"[MapDatabase] 리프레시 완료! 시작 맵: {startMapName}, 총 맵 개수: {mapNames.Count}");
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
