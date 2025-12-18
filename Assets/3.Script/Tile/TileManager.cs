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
    public TimeSO timeSO;

    [Header("특별 타일")]
    public List<GameObject> tileList;

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
        yield return new WaitForSeconds(timeSO.collapseDelay);

        RemoveTile(coord);

        yield return new WaitForSeconds(timeSO.respawnDelay);

        SpawnTile(coord);
    }

    // =========================
    // 시간마다 랜덤 타일 제거
    // =========================
    IEnumerator RandomTileChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeSO.dangerInterval);

            List<Vector2Int> keys = new List<Vector2Int>(activeTiles.Keys);

            for (int i = 0; i < timeSO.randomRemoveCount && keys.Count > 0; i++)
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

    public void SteppedTile(Vector2Int tileVector)
    {
        activeTiles[tileVector].GetComponent<HexTile>().OnStepped();
    }
}