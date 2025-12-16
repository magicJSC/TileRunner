using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ================================
// 육각 타일 무한 맵 생성기
// 자동 생성 / 자동 삭제 / 난이도 증가
// ================================
public class TileManager : MonoBehaviour
{
    [Header("타일 설정")]
    public GameObject tilePrefab;
    public int radius = 5;
    public float tileSize = 1f;

    [Header("시간 설정")]
    public float collapseDelay = 0.8f;   // 밟고 사라지기까지
    public float respawnDelay = 4.0f;    // 다시 생기기까지
    public float dangerInterval = 6.0f;  // 랜덤 제거 주기
    public int randomRemoveCount = 1;

    [Header("특별 타일")]
    public List<GameObject> tileList;

    // 현재 존재하는 타일
    Dictionary<Vector2Int, GameObject> activeTiles =
        new Dictionary<Vector2Int, GameObject>();

    public static TileManager Instance { get { if (instance == null) Init(); return instance; } set { instance = value; } }
    private static TileManager instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private static void Init()
    {
        Instance = FindFirstObjectByType<TileManager>();
    }

    void Start()
    {
        GenerateInitialMap();
        StartCoroutine(RandomTileChange());
    }

    // =========================
    // 초기 육각 맵 생성
    // =========================
    void GenerateInitialMap()
    {
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);

            for (int r = r1; r <= r2; r++)
            {
                Vector2Int coord = new Vector2Int(q, r);
                SpawnTile(coord);
            }
        }
    }

    // =========================
    // 타일 생성
    // =========================
    void SpawnTile(Vector2Int coord, GameObject spawnTile = null)
    {
        if (activeTiles.ContainsKey(coord))
            return;
        GameObject tileGo;

        if (spawnTile != null)
            tileGo = spawnTile;
        else
            tileGo = tilePrefab;

        Vector3 pos = AxialToWorld(coord);
        GameObject tile = Instantiate(tileGo, pos, Quaternion.identity, transform);

        tile.GetComponent<HexTile>().axialCoord = coord;
        activeTiles.Add(coord, tile);
    }

    // =========================
    // 타일 삭제
    // =========================
    void RemoveTile(Vector2Int coord)
    {
        if (!activeTiles.ContainsKey(coord))
            return;

        Destroy(activeTiles[coord]);
        activeTiles.Remove(coord);
    }

    // =========================
    // 플레이어가 밟았을 때 호출
    // =========================
    public void RequestTileCollapse(Vector2Int coord)
    {
        if (!activeTiles.ContainsKey(coord))
            return;

        StartCoroutine(CollapseRoutine(coord));
    }

    IEnumerator CollapseRoutine(Vector2Int coord)
    {
        yield return new WaitForSeconds(collapseDelay);

        RemoveTile(coord);

        yield return new WaitForSeconds(respawnDelay);

        SpawnTile(coord);
    }

    // =========================
    // 시간마다 랜덤 타일 제거
    // =========================
    IEnumerator RandomTileChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(dangerInterval);

            List<Vector2Int> keys = new List<Vector2Int>(activeTiles.Keys);

            for (int i = 0; i < randomRemoveCount && keys.Count > 0; i++)
            {
                int idx = UnityEngine.Random.Range(0, keys.Count);
                Vector2Int coord = keys[idx];

                RemoveTile(coord);
                keys.RemoveAt(idx);

                // 랜덤으로 특별 타일 생성
                int randomIdx = UnityEngine.Random.Range(0, tileList.Count);
                SpawnTile(coord, tileList[randomIdx]);
            }
        }
    }

    // =========================
    // Axial → World 좌표 변환
    // =========================
    Vector3 AxialToWorld(Vector2Int coord)
    {
        float x = tileSize * (Mathf.Sqrt(3f) * coord.x + Mathf.Sqrt(3f) / 2f * coord.y);
        float z = tileSize * (3f / 2f * coord.y);
        return new Vector3(x, 0f, z);
    }
}