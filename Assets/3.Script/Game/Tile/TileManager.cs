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
    public GameObject goalPrefab;
    public int radius = 5;
    public float tileSize = 1f;
    public Transform playerTransform;
    public GameObject tileMapParent;

    [Header("특별 타일")]
    public GameObject dangerTile;
    public GameObject scoreTilePrefab;
    public float scoreInterval;

    [Header("맵 데이터")]
    public HexMapSO currentMap;

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

    static readonly Vector2Int[] Corners =
    {
    new Vector2Int(-4,  4), // 4
    new Vector2Int(-4,  0), // 5
    new Vector2Int( 0, -4), // 0
    new Vector2Int( 4, -4), // 1
    new Vector2Int( 4,  0), // 2
    new Vector2Int( 0,  4), // 3
};

    public static TileManager Instance { get { if (instance == null) Init(); return instance; } set { instance = value; } }
    private static TileManager instance;

    private void Awake()
    {
        if(instance == null)
            Instance = this;
    }

    private static void Init()
    {
        instance = FindFirstObjectByType<TileManager>();
    }

    void Start()
    {
        LoadMap(MapDatabase.Instance.startMap ,0);
        
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

    public void DeleteMap()
    {
        if (tileMapParent == null)
            return;

        if (Application.isPlaying)
            Destroy(tileMapParent);
#if UNITY_EDITOR
        else
            DestroyImmediate(tileMapParent);
#endif

        tileMapParent = null;
        activeTiles.Clear();
    }


    public void OnReachGoal(Vector2Int goalCoord)
    {
        HexMapSO nextMap =
            MapDatabase.Instance.GetMapByScore(GameManager.Instance.Score);

        int rotation = GetRotationFromGoal(goalCoord);

        LoadMap(nextMap, rotation);
    }

    public void LoadMap(HexMapSO mapData, int rotationStep)
    {
        DeleteMap();

        currentMap = mapData;

        if (tileMapParent == null)
            tileMapParent = new GameObject("TileMap");

        foreach (var tile in mapData.tiles)
        {
            Vector2Int rotatedCoord =
                RotateCoord(tile.coord, rotationStep);

            switch (tile.type)
            {
                case TileType.Start:
                    SpawnStartTile(rotatedCoord);
                    break;

                case TileType.Obstacle:
                    SpawnTile(rotatedCoord, dangerTile);
                    break;

                case TileType.Normal:
                    SpawnTile(rotatedCoord);
                    break;

                case TileType.Goal:
                    SpawnTile(rotatedCoord, goalPrefab);
                    break;
            }
        }
    }

    int GetRotationFromGoal(Vector2Int goalCoord)
    {
        for (int i = 0; i < Corners.Length; i++)
        {
            if (Corners[i] == goalCoord)
                return i;   // i번 = i * 60도 회전
        }

        return 0; // fallback
    }

    Vector2Int RotateCoord(Vector2Int coord, int times)
    {
        Vector2Int result = coord;

        for (int i = 0; i < times; i++)
        {
            result = new Vector2Int(
                -result.y,
                result.x + result.y
            );
        }

        return result;
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

        Vector3 pos = AxialToWorld(coord) + new Vector3(playerTransform.transform.position.x,0, playerTransform.transform.position.z);
        GameObject tile = Instantiate(tileGo, pos, Quaternion.identity, tileMapParent.transform);

        tile.GetComponent<HexTile>().axialCoord = coord;
        activeTiles.Add(coord, tile);
    }

    void SpawnStartTile(Vector2Int coord)
    {
        if (activeTiles.ContainsKey(coord))
            return;
        
        Vector3 pos = AxialToWorld(coord) + new Vector3(playerTransform.transform.position.x, 0, playerTransform.transform.position.z);
        GameObject tile = Instantiate(startTilePrefab, pos, Quaternion.identity, tileMapParent.transform);

        tile.GetComponent<StartTile>().axialCoord = coord;
        activeTiles.Add(coord, tile);
    }

    // =========================
    // 타일 삭제
    // =========================
    public void RemoveTile(Vector2Int coord)
    {
        if (!activeTiles.ContainsKey(coord))
            return;

        Destroy(activeTiles[coord]);
        activeTiles.Remove(coord);
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
                AxialToWorld(coord) + new Vector3(playerTransform.transform.position.x, 0, playerTransform.transform.position.z);

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