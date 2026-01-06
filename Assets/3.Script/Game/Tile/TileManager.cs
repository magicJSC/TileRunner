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
    public GameObject startTilePrefab;
    public int radius = 5;
    public float tileSize = 1f;
    public Transform playerTransform;
    private Vector3 startPlayerPos;

    [Header("시간 설정")]
    public DifficultSO difficultSO;

    [Header("특별 타일")]
    public GameObject dangerTile;
    public GameObject scoreTilePrefab;
    public float scoreInterval;

    // 현재 존재하는 타일
    Dictionary<Vector2Int, GameObject> activeTiles =
        new Dictionary<Vector2Int, GameObject>();


    private static readonly Vector2Int[] HexDirections =
{
    new Vector2Int( 1,  0),
    new Vector2Int(-1,  0),
    new Vector2Int( 0,  1),
    new Vector2Int( 0, -1),
    new Vector2Int( 1, -1),
    new Vector2Int(-1,  1)
};

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
        startPlayerPos = playerTransform.position;
        GenerateInitialMap();
        
        GameManager.Instance.startGameAction += StartGame;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
    }


    private void StartGame()
    {
        //StartCoroutine(RandomTileChangeDanger());
        //StartCoroutine(RandomTileChangeScore());
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

                if(coord != new Vector2Int(0,0))
                    SpawnTile(coord);
                else
                    SpawnStartTile(coord);
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

        Vector3 pos = AxialToWorld(coord) + new Vector3(startPlayerPos.x,0, startPlayerPos.z);
        GameObject tile = Instantiate(tileGo, pos, Quaternion.identity, transform);

        tile.GetComponent<HexTile>().axialCoord = coord;
        activeTiles.Add(coord, tile);
    }

    void SpawnStartTile(Vector2Int coord)
    {
        if (activeTiles.ContainsKey(coord))
            return;
        
        Vector3 pos = AxialToWorld(coord) + new Vector3(startPlayerPos.x, 0, startPlayerPos.z);
        GameObject tile = Instantiate(startTilePrefab, pos, Quaternion.identity, transform);

        tile.GetComponent<StartTile>().axialCoord = coord;
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

    /// <summary>
    /// 점수 타일 생성
    /// </summary>
    /// <param name="coord"></param>
    void SpawnScoreTile(Vector2Int coord)
    {
        if (activeTiles.ContainsKey(coord))
            return;

        Vector3 pos = AxialToWorld(coord) + new Vector3(startPlayerPos.x, 0, startPlayerPos.z);
        GameObject tile = Instantiate(scoreTilePrefab, pos, Quaternion.identity, transform);

        var scoreTile = tile.GetComponent<RocketTile>();
        scoreTile.axialCoord = coord;
        //scoreTile.InitTrial(TrialPicker.Instance.PickTrial());
        activeTiles.Add(coord, tile);
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
        yield return new WaitForSeconds(difficultSO.collapseDelay);

        RemoveTile(coord);

        yield return new WaitForSeconds(difficultSO.respawnDelay);

        SpawnTile(coord);
    }

    // =========================
    // 시간마다 랜덤 타일 위험타일로 변경
    // =========================
    public void RandomTileChangeDanger()
    {
        List<Vector2Int> keys = new List<Vector2Int>(activeTiles.Keys);

        for (int i = 0; i < difficultSO.dangerCount && keys.Count > 0; i++)
        {
            int idx = UnityEngine.Random.Range(0, keys.Count);
            Vector2Int coord = keys[idx];

            RemoveTile(coord);
            keys.RemoveAt(idx);

            // 랜덤으로 특별 타일 생성
            SpawnTile(coord, dangerTile);
        }
    }

    /// <summary>
    /// 시간마다 랜덤 타일 점수타일로 변경
    /// </summary>
    IEnumerator RandomTileChangeScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(scoreInterval);

            List<Vector2Int> keys = new List<Vector2Int>(activeTiles.Keys);

            int idx = UnityEngine.Random.Range(0, keys.Count);
            Vector2Int coord = keys[idx];

            RemoveTile(coord);

            SpawnScoreTile(coord);
        }
    }

    // =========================
    // Axial → World 좌표 변환
    // =========================
    public Vector3 AxialToWorld(Vector2Int coord)
    {
        float x = tileSize * (Mathf.Sqrt(3f) * coord.x + Mathf.Sqrt(3f) / 2f * coord.y);
        float z = tileSize * (3f / 2f * coord.y);
        return new Vector3(x, 0f, z);
    }


    /// <summary>
    /// 근첩한 타일 좌표 반환
    /// </summary>
    /// <param name="center"></param>
    public List<Vector2Int> GetNeighborCoords(Vector2Int center)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        foreach (var dir in HexDirections)
        {
            Vector2Int neighbor = center + dir;

            if (activeTiles.ContainsKey(neighbor))
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    public List<Vector2Int> GetRandomActivateCoord(int count = 1)
    {
        List<Vector2Int> keys = new List<Vector2Int>(activeTiles.Keys);

        List<Vector2Int> coordList = new List<Vector2Int>();

        for (int i = 0; i < count; i++)
        {
            int idx = UnityEngine.Random.Range(0, keys.Count);
            Vector2Int coord = keys[idx];
            keys.RemoveAt(idx);

            coordList.Add(coord);
        }
        return coordList;
    }

    /// <summary>
    /// Vector2Int값으로 타일을 밞음 처리
    /// </summary>
    /// <param name="tileVector"></param>
    public void SteppedTile(Vector2Int tileVector)
    {
        activeTiles[tileVector].GetComponent<HexTile>().OnStepped();
    }

    /// <summary>
    /// 타일 사라지게 처리
    /// </summary>
    /// <param name="tileVector"></param>
    public void DissapearTile(Vector2Int tileVector)
    {
        activeTiles[tileVector].GetComponent<HexTile>().Disappear();
    }

    public GameObject GetTile(Vector2Int tileVector)
    {
        return activeTiles[tileVector];
    }

    public GameObject GetRandomTile()
    {
        List<GameObject> values = new List<GameObject>(activeTiles.Values);
        int idx = UnityEngine.Random.Range(0, values.Count);
        return values[idx];
    }

    public Vector3 GetRandomTilePosition(
     Transform target,
     float minDistance,
     float maxDistance)
    {
        List<Vector3> candidates = new List<Vector3>();
        Vector3 targetPos = target.position;

        // 타일이 존재할 수 있는 좌표 범위 (초기 맵 기준)
       foreach(var coord in activeTiles.Keys)
        {
            Vector3 worldPos =
                AxialToWorld(coord) + new Vector3(startPlayerPos.x, 0, startPlayerPos.z);

            float dist = Vector3.Distance(targetPos, worldPos);

            if (dist >= minDistance && dist < maxDistance)
            {
                candidates.Add(worldPos);
            }
        }

        if (candidates.Count == 0)
            return Vector3.zero;

        int idx = UnityEngine.Random.Range(0, candidates.Count);
        return candidates[idx];
    }

}